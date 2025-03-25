using HarvestDataViewerDomain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HarvestDataViewerDomain.Data;

internal sealed class Context : DbContext
{
  public Context(DbContextOptions<Context> options) : base(options)
  {
    Database.EnsureCreated();
  }

  public DbSet<WeighingRecord> WeighingRecords { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    var firstWeighingRecord = new WeighingRecord
    {
      Id = 1,
      CarNumber = "A123BC",
      GrossWeight = 5000,
      TareWeight = 2000,
      TareDate = new DateTime(2023, 10, 1),
      GrossDate = new DateTime(2023, 10, 1)
    };

    var secondWeighingRecord = new WeighingRecord
    {
      Id = 2,
      CarNumber = "B456DF",
      GrossWeight = 6000,
      TareWeight = 2500,
      TareDate = new DateTime(2023, 10, 2),
      GrossDate = new DateTime(2023, 10, 2)
    };

    var thirdWeighingRecord = new WeighingRecord
    {
      Id = 3,
      CarNumber = "C789GH",
      GrossWeight = 5500,
      TareWeight = 2200,
      TareDate = new DateTime(2023, 10, 3),
      GrossDate = new DateTime(2023, 10, 3)
    };

    // Добавляем начальные данные для таблицы WeighingRecords
    modelBuilder.Entity<WeighingRecord>().HasData(firstWeighingRecord, secondWeighingRecord, thirdWeighingRecord);

    base.OnModelCreating(modelBuilder);
  }
}