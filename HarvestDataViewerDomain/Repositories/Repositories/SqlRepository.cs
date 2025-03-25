using HarvestDataViewerDomain.Data;
using HarvestDataViewerDomain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HarvestDataViewerDomain.Repositories.Repositories;

/// <summary>
///   <inheritdoc cref="IRepository{T}" />
/// </summary>
/// <param name="context"></param>
/// <typeparam name="T"></typeparam>
internal class SqlRepository<T>(Context context) : IRepository<T> where T : class
{
  private readonly DbSet<T> _dbSet = context.Set<T>();

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task<T?> GetByIdAsync(int id)
  {
    return await _dbSet.FindAsync(id);
  }

  public async Task AddAsync(T item)
  {
    await _dbSet.AddAsync(item);
    await context.SaveChangesAsync();
  }

  public async Task UpdateAsync(T item)
  {
    _dbSet.Update(item);
    await context.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var user = await _dbSet.FindAsync(id);

    if (user == null)
      return;

    _dbSet.Remove(user);

    await context.SaveChangesAsync();
  }
}