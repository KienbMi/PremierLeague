using PremierLeague.Wpf.Common;
using PremierLeague.Wpf.ViewModels;
using System.Windows;

namespace PremierLeague.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private async void Application_Startup(object sender, StartupEventArgs e)
    {
      WindowController controller = new WindowController();
      controller.ShowWindow(await MainViewModel.CreateAsync(controller));
    }
  }
}
