using Avalonia.Controls;
using HarvestDataViewer.MVVM.ViewModels;

namespace HarvestDataViewer.MVVM.Views;

public partial class DataView : UserControl
{
  public DataView(DataViewModel dataViewModel)
  {
    InitializeComponent();

    var viewModel = dataViewModel;
    DataContext = viewModel;

    Loaded += async (_, _) => await viewModel.InitializeAsync();
  }
}