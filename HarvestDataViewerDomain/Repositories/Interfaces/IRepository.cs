namespace HarvestDataViewerDomain.Repositories.Interfaces;

/// <summary>
///   Репозиторий для работы с данными
/// </summary>
public interface IRepository<T>
  where T : class
{
  /// <summary>
  ///   Логика получения сущностей
  /// </summary>
  /// <returns></returns>
  public Task<IEnumerable<T>> GetAllAsync();

  /// <summary>
  ///   Логика получения сущности по Id
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  public Task<T?> GetByIdAsync(int id);

  /// <summary>
  ///   Логика добавления сущности
  /// </summary>
  /// <param name="item"></param>
  /// <returns></returns>
  public Task AddAsync(T item);

  /// <summary>
  ///   Логика обновления сущности
  /// </summary>
  /// <param name="item"></param>
  /// <returns></returns>
  public Task UpdateAsync(T item);

  /// <summary>
  ///   Логика удаления сущности по Id
  /// </summary>
  /// <param name="id"></param>
  /// <returns></returns>
  public Task DeleteAsync(int id);
}