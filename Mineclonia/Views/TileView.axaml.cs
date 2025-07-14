using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Mineclonia.ViewModels;

namespace Mineclonia.Views;

public partial class TileView : UserControl
{
    ICommand? TileRightClickCommand { get; set; }
    
    public TileView()
    {
        InitializeComponent();
    }


    private void Tile_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (this.DataContext is not TileViewModel tileViewModel) return;
        
        if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
        {
            tileViewModel.RightClickedCommand?.Execute(tileViewModel);
            e.Handled = true;
        }
    }
}