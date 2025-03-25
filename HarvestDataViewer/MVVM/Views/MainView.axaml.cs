using Avalonia.Controls;

namespace HarvestDataViewer.MVVM.Views;

public partial class MainView : Window
{
  public MainView()
  {
    InitializeComponent();
  }

  public void SetContent(Control content)
  {
    MainContent.Content = content;
  }
}