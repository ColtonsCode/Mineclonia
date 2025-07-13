using System.Collections.ObjectModel;
using Minecodel;

namespace Mineclonia.ViewModels;

public class MineFieldViewModel : ViewModelBase
{
    private Minefield _field;

    public ObservableCollection<TileViewModel> Tiles { get; } = [];
    
    public int Rows { get; set; }
    
    public int Columns { get; set; }

    // Parameterless constructor for design time.
    public MineFieldViewModel()
        : this(new Minefield(10, 10))
    {}

    public MineFieldViewModel(Minefield field)
    {
        _field = field;
        Rows = field.Tiles.GetLength(0);
        Columns = field.Tiles.GetLength(1);
        
        foreach (var tile in field.Tiles)
        {
            Tiles.Add(new TileViewModel(tile));
        }
    }
}