using HarvestDataViewerDomain.Data.Models;
using HarvestDataViewerDomain.Exceptions;
using HarvestDataViewerDomain.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace HarvestDataViewerDomain.Services;

public class WeighingRecordService(
  IRepository<WeighingRecord> weighingRecordRepository,
  ILogger<WeighingRecordService> logger)
{
  private const string WeighingRecordFindText = "Запись взвешивания не найдена.";
  private const string InvalidValueText = "Недопустимое значение.";

  /// <summary>
  ///   Добавление записи взвешивания
  /// </summary>
  /// <param name="carNumber"></param>
  /// <param name="gross"></param>
  /// <param name="tare"></param>
  /// <param name="tareDate"></param>
  /// <param name="grossDate"></param>
  /// <returns></returns>
  /// <exception cref="InvalidValueException"></exception>
  public async Task<WeighingRecord> AddWeighingRecordAsync(string carNumber, string gross, string tare,
    DateTimeOffset tareDate, DateTimeOffset grossDate)
  {
    if (!int.TryParse(gross, out _) || !int.TryParse(tare, out _))
    {
      logger.LogError("Invalid value gross weigh or tare weigh.");
      throw new InvalidValueException(InvalidValueText);
    }

    var newWeighingRecord = new WeighingRecord
    {
      CarNumber = carNumber,
      TareDate = tareDate.DateTime,
      GrossDate = grossDate.DateTime,
      GrossWeight = int.Parse(gross),
      TareWeight = int.Parse(tare)
    };

    await weighingRecordRepository.AddAsync(newWeighingRecord);

    logger.LogInformation("Added weighing record");
    return newWeighingRecord;
  }

  /// <summary>
  ///   Логика получения всех записей взвешивания
  /// </summary>
  /// <returns></returns>
  public Task<IEnumerable<WeighingRecord>> GetAllWeighingRecordsAsync()
  {
    logger.LogInformation("Retrieve a list of all weighing records...");
    return weighingRecordRepository.GetAllAsync();
  }

  /// <summary>
  ///   Логика поиска записи взвешивания по номеру машины
  /// </summary>
  /// <param name="number"></param>
  /// <returns></returns>
  public async Task<WeighingRecord> FindWeighingRecordAsync(string number)
  {
    logger.LogInformation("Finding weighing records...");
    var weighingRecords = (await GetAllWeighingRecordsAsync()).ToList();
    logger.LogInformation("Search for a weighing record by car number...");

    var weighingRecord = weighingRecords.FirstOrDefault(x => x.CarNumber == number);

    if (weighingRecord != null)
      return weighingRecord;

    logger.LogError("Could not find a weighing record.");
    throw new WeighingRecordNotFoundException(WeighingRecordFindText);
  }
}