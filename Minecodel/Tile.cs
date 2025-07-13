using System.Runtime.InteropServices;

namespace Minecodel;

[StructLayout(LayoutKind.Sequential)]
public struct Tile
{
    public bool IsMine;
    public bool IsRevealed;
    public bool IsFlagged;
    public int AdjacentMines;
}