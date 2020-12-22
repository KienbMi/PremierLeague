namespace PremierLeague.Wpf.Common.Contracts
{
  public interface IWindowController
  {
    void ShowWindow(BaseViewModel viewModel, bool showAsModal);
    void CloseWindow(BaseViewModel viewModel);
  }
}
