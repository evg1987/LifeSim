using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LifeSim;

/// <summary>
/// Base class for 2-dimensional grid array
/// </summary>
/// <typeparam name="T">Cell type</typeparam>
public abstract class GridCellsLayout<T>
{
	/// <summary>
	/// Number of cells (horizontal)
	/// </summary>
	public readonly int Width;

	/// <summary>
	/// Number of cells (vertical)
	/// </summary>
	public readonly int Height;

	/// <summary>
	/// The cells [Width, Height]
	/// </summary>
	public T[,] Cells { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="width">Number of cells (horizontal)</param>
    /// <param name="height">Number of cells (vertical)</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public GridCellsLayout(int width, int height)
	{
		if (width < 1 || height < 1)
		{
			throw new ArgumentOutOfRangeException($"Invalid {nameof(width)} or {nameof(height)}. Ensure that these values > 0");
		}

		Width = width;
		Height = height;
	}

	/// <summary>
	/// Construct the cell
	/// </summary>
	/// <param name="x">Cell X coordinate (horizontal)</param>
	/// <param name="y">Cell Y coordinate (vertical)</param>
	protected abstract T ConstructCell(int x, int y);

    /// <summary>
    /// Begin cells construction. This method must be called manually in child classes
    /// </summary>
    protected virtual void InitializeCells()
	{
		Cells = new T[Width, Height];
		for (int h = 0; h < Height; h++)
		{
			for (int w = 0; w < Width; w++)
			{
				Cells[w, h] = ConstructCell(w, h);
			}
		}
	}

	/// <summary>
	/// Enumerate each cell left -> right, up -> down
	/// </summary>
	public IEnumerable<T> EnumerateCells()
	{
        for (int h = 0; h < Height; h++)
        {
            for (int w = 0; w < Width; w++)
            {
				yield return Cells[w, h];
            }
        }
    }

    /// <summary>
    /// Check if cell coordinates within array bounds
    /// </summary>
    public bool IsValidCoords(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    /// <summary>
    /// Check if cell coordinates within array bounds
    /// </summary>
    public bool IsValidCoords(Point coords)
    {
        return IsValidCoords(coords.X, coords.Y);
    }
}
