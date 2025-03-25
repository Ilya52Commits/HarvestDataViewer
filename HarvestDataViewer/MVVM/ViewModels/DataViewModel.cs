using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HarvestDataViewerDomain.Data.Models;
using HarvestDataViewerDomain.Exceptions;
using HarvestDataViewerDomain.Services;

namespace HarvestDataViewer.MVVM.ViewModels;

public partial class DataViewModel(WeighingRecordService weighingRecordService, ChartViewModel chartViewModel)
  : ObservableObject
{
  [ObservableProperty] private string _carNumber = string.Empty;

  [ObservableProperty] private string _errorMessageCarNumber = string.Empty;
  [ObservableProperty] private string _errorMessageWeight = string.Empty;

  [ObservableProperty] private string _foundCar = string.Empty;
  [ObservableProperty] private DateTimeOffset _grossDate = DateTimeOffset.Now;
  [ObservableProperty] private string _grossWeight = string.Empty;
  [ObservableProperty] private DateTimeOffset _tareDate = DateTimeOffset.Now;
  [ObservableProperty] private string _tareWeight = string.Empty;
  [ObservableProperty] private ObservableCollection<WeighingRecord> _weighingRecords = [];
  public ChartViewModel ChartViewModel { get; } = chartViewModel;

  /// <summary>
  ///   Инициализация страницы
  /// </summary>
  public async Task InitializeAsync()
  {
    await LoadWeighingRecordsAsync();
  }

  /// <summary>
  ///   Загрузка данных
  /// </summary>
  private async Task LoadWeighingRecordsAsync()
  {
    WeighingRecords = new ObservableCollection<WeighingRecord>(
      (await weighingRecordService.GetAllWeighingRecordsAsync()).ToList()
    );
    ChartViewModel.UpdatePlot(WeighingRecords);
  }

  /// <summary>
  ///   Обновление таблицы и графика при очистке значения поиска
  /// </summary>
  /// <param name="value"></param>
  partial void OnFoundCarChanged(string value)
  {
    if (!string.IsNullOrWhiteSpace(value)) return;
    _ = LoadWeighingRecordsAsync();
  }

  /// <summary>
  ///   Поиск записи взвешивания
  /// </summary>
  [RelayCommand]
  private async Task FindWeighingRecordsAsync()
  {
    ErrorMessageCarNumber = string.Empty;
    if (FoundCar == string.Empty) return;
    try
    {
      var foundRecord = await weighingRecordService.FindWeighingRecordAsync(FoundCar);
      WeighingRecords.Clear();
      WeighingRecords.Add(foundRecord);
      ChartViewModel.UpdatePlot(WeighingRecords);
    }
    catch (WeighingRecordNotFoundException e)
    {
      ErrorMessageCarNumber = e.Message;
    }
  }

  /// <summary>
  ///   Добавление записи взвешивания
  /// </summary>
  [RelayCommand]
  private async Task AddNewWeighingRecordAsync()
  {
    ErrorMessageCarNumber = string.Empty;
    ErrorMessageWeight = string.Empty;
    try
    {
      var newWeighingRecord =
        await weighingRecordService.AddWeighingRecordAsync(CarNumber, GrossWeight, TareWeight, TareDate, GrossDate);
      WeighingRecords.Add(newWeighingRecord);
      ChartViewModel.AddDataPoint(newWeighingRecord);
    }
    catch (InvalidValueException e)
    {
      ErrorMessageWeight = e.Message;
    }
  }
}