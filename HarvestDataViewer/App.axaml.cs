using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using HarvestDataViewer.MVVM.ViewModels;
using HarvestDataViewer.MVVM.Views;
using HarvestDataViewerDomain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OxyPlot.Avalonia;
using Serilog;

namespace HarvestDataViewer;

public class App : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
    OxyPlotModule.EnsureLoaded();
  }

  public override void OnFrameworkInitializationCompleted()
  {
    Log.Logger = new LoggerConfiguration()
      .MinimumLevel.Debug()
      .WriteTo.Console()
      .CreateLogger();

    var configuration = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json")
      .Build();

    var loggerFactory = LoggerFactory.Create(static loggingBuilder => { loggingBuilder.AddSerilog(); });

    var logger = loggerFactory.CreateLogger("Program");

    var serviceCollection = new ServiceCollection();

    serviceCollection.AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(); });

    ServiceConfigurator.ConfigureServices(serviceCollection, configuration, logger);

    logger.LogInformation("Adding Views and ViewModels services...");
    logger.LogInformation("Adding ChartViewModel...");
    serviceCollection.AddTransient<ChartViewModel>();
    logger.LogInformation("Adding DataViewModel...");
    serviceCollection.AddTransient<DataViewModel>();
    logger.LogInformation("Adding DataView...");
    serviceCollection.AddTransient<DataView>();

    var serviceProvider = serviceCollection.BuildServiceProvider();

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      DisableAvaloniaDataAnnotationValidation();

      var mainView = new MainView();

      var dataView = serviceProvider.GetService<DataView>();

      logger.LogInformation("Opening main view...");
      mainView.SetContent(dataView ?? throw new InvalidOperationException("Main data view is null"));
      desktop.MainWindow = mainView;
    }

    base.OnFrameworkInitializationCompleted();
  }

  private static void DisableAvaloniaDataAnnotationValidation()
  {
    // Get an array of plugins to remove
    var dataValidationPluginsToRemove =
      BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

    // remove each entry found
    foreach (var plugin in dataValidationPluginsToRemove) BindingPlugins.DataValidators.Remove(plugin);
  }
}