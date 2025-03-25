using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using HarvestDataViewerDomain.Data.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace HarvestDataViewer.MVVM.ViewModels;

public partial class ChartViewModel : ObservableObject
{
  [ObservableProperty] private PlotModel _plotModel = new() { Title = "График изменения веса нетто по датам" };

  public ChartViewModel()
  {
    InitializeAxes();
  }

  /// <summary>
  ///   Инициализация осей графика
  /// </summary>
  private void InitializeAxes()
  {
    PlotModel.Axes.Add(new DateTimeAxis
    {
      Position = AxisPosition.Bottom,
      StringFormat = "dd.MM.yyyy",
      Title = "Дата"
    });

    PlotModel.Axes.Add(new LinearAxis
    {
      Position = AxisPosition.Left,
      Title = "Вес нетто (кг)"
    });
  }

  /// <summary>
  ///   Обновление данных графика
  /// </summary>
  public void UpdatePlot(IEnumerable<WeighingRecord> records)
  {
    var series = new LineSeries
    {
      MarkerType = MarkerType.Circle,
      MarkerSize = 4,
      MarkerStroke = OxyColors.Black
    };

    foreach (var record in records.OrderBy(r => r.GrossDate))
      if (record is { GrossDate: not null, NetWeight: not null })
        series.Points.Add(new DataPoint(
          DateTimeAxis.ToDouble(record.GrossDate.Value),
          record.NetWeight.Value
        ));

    PlotModel.Series.Clear();
    PlotModel.Series.Add(series);
    PlotModel.InvalidatePlot(true);
  }

  /// <summary>
  ///   Добавление новой точки
  /// </summary>
  public void AddDataPoint(WeighingRecord record)
  {
    if (record is not { GrossDate: not null, NetWeight: not null })
      return;

    if (PlotModel.Series.FirstOrDefault() is not LineSeries series)
    {
      series = new LineSeries
      {
        MarkerType = MarkerType.Circle,
        MarkerSize = 4,
        MarkerStroke = OxyColors.Black
      };
      PlotModel.Series.Add(series);
    }

    series.Points.Add(new DataPoint(
      DateTimeAxis.ToDouble(record.GrossDate.Value),
      record.NetWeight.Value
    ));

    PlotModel.InvalidatePlot(true);
  }
}