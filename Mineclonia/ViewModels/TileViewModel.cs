using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using Minecodel;

namespace Mineclonia.ViewModels;

public class TileViewModel(Tile tile) : ViewModelBase
{
    public static Bitmap BombImage { get; } = new (AssetLoader.Open(new Uri("avares://Mineclonia/Assets/bomb100.png")));
    public static Bitmap FlagImage { get; } = new (AssetLoader.Open(new Uri("avares://Mineclonia/Assets/flag.png")));
    
    private Tile _tile = tile;
    
    public List<TileViewModel> AdjacentTiles { get; } = [];
    
    public ICommand? ClickedCommand { get; set; }
    
    public ICommand? RightClickedCommand { get; set; }
    
    public bool ShowBomb => IsRevealed && IsMine;
    
    public bool ShowFlag => !IsRevealed && IsFlagged;

    
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
                return string.Empty;
            }

            if (AdjacentMines == 0)
            {
                return string.Empty;
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
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(IsClickable));
            OnPropertyChanged(nameof(BackgroundBrush));
            OnPropertyChanged(nameof(ShowBomb));
            OnPropertyChanged(nameof(ShowFlag));
        }
    }
    
    public bool IsClickable => !IsRevealed;

    public bool IsFlagged
    {
        get => _tile.IsFlagged;
        set
        {
            if (_tile.IsFlagged == value) return;
            _tile.IsFlagged = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ShowFlag));
        }
    }
    
    public IImmutableSolidColorBrush BackgroundBrush
    {
        get
        {
            if (!IsRevealed)
            {
                return Brushes.DimGray;
            }

            if (ShowBomb)
            {
                return Brushes.DarkRed;
            }
            
            if (AdjacentMines == 0)
                return Brushes.DarkGray;
            
            // Create a slightly lighter color that dark gray
            var dimGrayColor = (Color)Brushes.DarkGray.Color;
            var lighterColor = new Color(255, 
                (byte)Math.Min(255, dimGrayColor.R * 0.9), 
                (byte)Math.Min(255, dimGrayColor.G * 0.9), 
                (byte)Math.Min(255, dimGrayColor.B * 0.9));
            return new ImmutableSolidColorBrush(lighterColor);
            
            
        }
    }

    public IImmutableSolidColorBrush ForegroundBrush
    {
        get
        {
            switch (AdjacentMines)
            {
                case 1:
                    return Brushes.Blue;
                case 2:
                    return Brushes.LawnGreen;
                case 3:
                    return Brushes.OrangeRed;
                case 4:
                    return Brushes.BlueViolet;
                case 5:
                    return Brushes.Maroon;
                case 6:
                    return Brushes.Aqua;
                case 7:
                    return Brushes.Gold;
                case 8:
                    return Brushes.Indigo;
                default:
                    return Brushes.Black;
            }
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