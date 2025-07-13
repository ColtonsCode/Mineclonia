using Avalonia.Controls;
using Mineclonia.ViewModels;
using Minecodel;

namespace Mineclonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var mineField = new Minefield(10, 25);
        MineFieldView.DataContext = new MineFieldViewModel(mineField);
    }
}