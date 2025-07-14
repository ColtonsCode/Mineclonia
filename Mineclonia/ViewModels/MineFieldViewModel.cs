using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Minecodel;

namespace Mineclonia.ViewModels;

public partial class MineFieldViewModel : ViewModelBase
{
    private Minefield _field;

    public ObservableCollection<TileViewModel> Tiles { get; } = [];
    
    public int Rows { get; set; }
    
    public int Columns { get; set; }

    // Parameterless constructor for design time.
    public MineFieldViewModel()
        : this(new Minefield(10, 10, 10))
    {}

    public MineFieldViewModel(Minefield field)
    {
        _field = field;
        Rows = field.Tiles.GetLength(0);
        Columns = field.Tiles.GetLength(1);


        foreach (var tile in field.Tiles)
        {
            var vm = new TileViewModel(tile);
            vm.ClickedCommand = TileClickedCommand;
            vm.RightClickedCommand = TileRightClickedCommand;
            
            Tiles.Add(vm);
        }
        
        // Awesome and clean code that gives each tile view model a list of references to
        // its neighboring tile view models that definitely wasn't an after-thought
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                int index = row * Columns + col;
                var tile = Tiles[index];

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;

                        int r = row + i;
                        int c = col + j;
                        if (r < 0 || r >= Rows || c < 0 || c >= Columns)
                            continue;

                        int adjIndex = r * Columns + c;
                        var adjTile = Tiles[adjIndex];
                        tile.AdjacentTiles.Add(adjTile);
                    }
                }
            }
        }
    }

    [RelayCommand]
    private void TileClicked(TileViewModel? tile)
    {
        if (tile is null) return;
        if (tile.IsRevealed || tile.IsFlagged) return;
        
        if (tile.IsMine)
        {
            tile.IsRevealed = true;
            return;
        }
        
        FloodRevealAdjacents(tile);
    }

    [RelayCommand]
    private void TileRightClicked(TileViewModel? tile)
    {
        if (tile is null) return;
        if (tile.IsRevealed) return;
        tile.IsFlagged = !tile.IsFlagged;
    }
    
    private void FloodRevealAdjacents(TileViewModel tile)
    {
        if (tile.IsRevealed) return;
        
        tile.IsRevealed = true;
        
        if (tile.AdjacentMines > 0) return;
        
        foreach (var adjTile in tile.AdjacentTiles)
        {
            FloodRevealAdjacents(adjTile);
        }
    }
}