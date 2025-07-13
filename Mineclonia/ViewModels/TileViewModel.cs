using Minecodel;

namespace Mineclonia.ViewModels;

public class TileViewModel(Tile tile) : ViewModelBase
{
    private Tile _tile = tile;

    public string DisplayText
    {
        get
        {
            if (!IsRevealed)
            {
                return string.Empty;
            }

            if (IsMine)
            {
                return "X";
            }

            return AdjacentMines.ToString();
        }
    }
    
    public bool IsMine => _tile.IsMine;
    
    public int AdjacentMines => _tile.AdjacentMines;

    public bool IsRevealed
    {
        get => _tile.IsRevealed;
        set
        {
            if (_tile.IsRevealed == value) return;
            _tile.IsRevealed = value;
            OnPropertyChanged();
        }
    }

    public bool IsFlagged
    {
        get => _tile.IsFlagged;
        set
        {
            if (_tile.IsFlagged == value) return;
            _tile.IsFlagged = value;
            OnPropertyChanged();
        }
    }
    
    public TileViewModel() : this(new Tile()
    {
        IsMine = false,
        AdjacentMines = 7,
        IsRevealed = true,
        IsFlagged = false
    })
    {
    }
}