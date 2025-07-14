namespace Mineclonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    public MineFieldViewModel MineField { get; } = new();

    public MainWindowViewModel()
    {
        // Parameterless constructor for design time.
    }
    public MainWindowViewModel(MineFieldViewModel mfvm)
    {
        MineField = mfvm;
    }
}