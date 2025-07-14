using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Minecodel;

public class Minefield
{
    private const int MbSize = 1024 * 1024;
    private const int WarningSize = 500 * MbSize; // 500 MB
    
    public Tile[,] Tiles { get; }

    public Minefield(int rows, int cols, int mines)
    {
        int tileSize = Unsafe.SizeOf<Tile>();
        int fieldSize = tileSize * rows * cols;
        long sizeBeforeAlloc = GC.GetAllocatedBytesForCurrentThread();
        
        Tiles = GenerateTiles(rows, cols, mines);
        long actualSize = GC.GetAllocatedBytesForCurrentThread() -  sizeBeforeAlloc;
    }

    private static Tile[,] GenerateTiles(int rows, int cols, int totalMines)
    {
        var rng = new Random();
        
        var tiles = new Tile[rows, cols];

        var positions = new List<(int row, int col)>(rows * cols);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                positions.Add((row, col));
            }
        }

        // Fisher-Yates shuffle.
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }
        
        for (int i = 0; i < totalMines; i++)
        {
            (int row, int col) = positions[i];
            tiles[row, col].IsMine = true;
            // Update the mine count for adjacent tiles.
            for (int drow = -1; drow <= 1; drow++)
            {
                for (int dcol = -1; dcol <= 1; dcol++)
                {
                    if (dcol == 0 && drow == 0) continue;
                    int ncol = col + dcol;
                    int nrow = row + drow;
                    if (ncol < 0 || ncol >= cols || nrow < 0 || nrow >= rows) continue;
                    if (!tiles[nrow, ncol].IsMine)
                        tiles[nrow, ncol].AdjacentMines++;
                }
            }
        }
        
        return tiles;
    }
    
    private static void PrintDebugInfo(
        int width,
        int height,
        int tileSize,
        long estimatedSize,
        long actualSize,
        int totalMines)
    {
        Debug.WriteLine("🔍 Minefield Debug Info");
        Debug.WriteLine(new string('-', 40));
        Debug.WriteLine($" Dimensions      : {width} x {height}");
        Debug.WriteLine($" Total tiles     : {width * height:n0}");
        Debug.WriteLine($" Mine count      : {totalMines:n0}");
        Debug.WriteLine($" Tile size       : {tileSize:n0} bytes");
        Debug.WriteLine($" Estimated size  : {estimatedSize:n0} bytes  ({estimatedSize / MbSize:n0} MB)");
        Debug.WriteLine($" Actual size     : {actualSize:n0} bytes  ({actualSize / MbSize:n0} MB)");
        Debug.WriteLine($" Max allowed     : {WarningSize:n0} bytes  ({WarningSize / MbSize:n0} MB)");

        double percent = ((double)estimatedSize / WarningSize) * 100;
        Debug.WriteLine($" Memory usage    : {percent:n2}% of limit");
        Debug.WriteLine(new string('-', 40));
    }
    
    private static void PrintBoardPreview(Tile[,] tiles, int maxPreviewRows = 20, int maxPreviewCols = 50)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);

        int displayHeight = Math.Min(height, maxPreviewRows);
        int displayWidth = Math.Min(width, maxPreviewCols);

        Debug.WriteLine("Board Preview (X = mine, number = adjacent mine count)");
        Debug.WriteLine($"Showing top-left {displayWidth} x {displayHeight} section:");
        Debug.WriteLine(new string('-', displayWidth));

        for (int y = 0; y < displayHeight; y++)
        {
            string line = "";
            for (int x = 0; x < displayWidth; x++)
            {
                var tile = tiles[x, y];
                if (tile.IsMine)
                    line += "X";
                else
                    line += tile.AdjacentMines.ToString();
            }
            Debug.WriteLine(line);
        }

        if (height > displayHeight || width > displayWidth)
            Debug.WriteLine("... (board truncated for preview)");
    }


}