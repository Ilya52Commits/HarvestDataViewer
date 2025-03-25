using HarvestDataViewerDomain.Data;
using HarvestDataViewerDomain.Data.Models;
using HarvestDataViewerDomain.Repositories.Interfaces;
using HarvestDataViewerDomain.Repositories.Repositories;
using HarvestDataViewerDomain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HarvestDataViewerDomain;

public static class ServiceConfigurator
{
  public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
  {
    logger.LogInformation("Adding domain services...");
    logger.LogInformation("Adding context...");
    services.AddDbContext<Context>(options =>
    {
      var dataProvider = configuration["DataProvider"];

      switch (dataProvider)
      {
        case "MsSql":
          logger.LogInformation("Using MsSql context.");
          options.UseLazyLoadingProxies().UseSqlServer(configuration["MsSqlDatabase"]);
          break;
        case "PgSql":
          logger.LogInformation("Using PgSql context.");
          options.UseLazyLoadingProxies().UseNpgsql(configuration["PgSqlDatabase"]);
          break;
      }
    });

    var dataProvider = configuration["DataProvider"];

    switch (dataProvider)
    {
      case "MsSql":
      case "PgSql":
        logger.LogInformation("Adding repository...");
        services.AddScoped<IRepository<WeighingRecord>, SqlRepository<WeighingRecord>>();
        break;
    }

    logger.LogInformation("Adding database services...");

    services.AddSingleton<WeighingRecordService>(provider =>
    {
      var weighingRecordRepository = provider.GetRequiredService<IRepository<WeighingRecord>>();
      var serviceLogger = provider.GetRequiredService<ILogger<WeighingRecordService>>();
      return new WeighingRecordService(weighingRecordRepository, serviceLogger);
    });
  }
}