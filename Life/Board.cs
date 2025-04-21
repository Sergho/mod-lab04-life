using System;
using System.Runtime.InteropServices;

namespace cli_life;

public class Board
{
	public readonly Cell[,] Cells;
	public readonly int CellSize;

	public int Columns { get { return Cells.GetLength(0); } }
	public int Rows { get { return Cells.GetLength(1); } }
	public int Width { get { return Columns * CellSize; } }
	public int Height { get { return Rows * CellSize; } }

	public Board(int width, int height, int cellSize, double liveDensity = .1)
	{
		CellSize = cellSize;

		Cells = new Cell[width / cellSize, height / cellSize];
		for (int x = 0; x < Columns; x++)
			for (int y = 0; y < Rows; y++)
				Cells[x, y] = new Cell();

		ConnectNeighbors();
		Randomize(liveDensity);
	}

	readonly Random rand = new Random();
	public void Randomize(double liveDensity)
	{
		foreach (var cell in Cells)
			cell.IsAlive = rand.NextDouble() < liveDensity;
	}
	public string Render(string alive, string notAlive)
	{
		string result = "";
		for (int row = 0; row < Rows; row++)
		{
			for (int col = 0; col < Columns; col++)
			{
				var cell = Cells[col, row];
				if (cell.IsAlive) result += alive;
				else result += notAlive;
			}
			result += "\n";
		}
		return result;
	}
	public string Serialize()
	{
		return Render("1", "0");
	}
	public void Deserialize(string data)
	{
		this.Clear();
		int row = 0;
		int col = 0;
		foreach (char c in data)
		{
			if (c == '\n')
			{
				row++;
				col = 0;
				continue;
			}
			if (col >= Columns || row >= Rows) throw new Exception("Incorrect file structure");
			var cell = Cells[col, row];
			cell.IsAlive = c == '1';
			col++;
		}
	}
	public void Clear()
	{
		foreach (var cell in Cells) cell.IsAlive = false;
	}

	public void Advance()
	{
		foreach (var cell in Cells)
			cell.DetermineNextLiveState();
		foreach (var cell in Cells)
			cell.Advance();
	}
	private void ConnectNeighbors()
	{
		for (int x = 0; x < Columns; x++)
		{
			for (int y = 0; y < Rows; y++)
			{
				int xL = (x > 0) ? x - 1 : Columns - 1;
				int xR = (x < Columns - 1) ? x + 1 : 0;

				int yT = (y > 0) ? y - 1 : Rows - 1;
				int yB = (y < Rows - 1) ? y + 1 : 0;

				Cells[x, y].Neighbors.Add(Cells[xL, yT]);
				Cells[x, y].Neighbors.Add(Cells[x, yT]);
				Cells[x, y].Neighbors.Add(Cells[xR, yT]);
				Cells[x, y].Neighbors.Add(Cells[xL, y]);
				Cells[x, y].Neighbors.Add(Cells[xR, y]);
				Cells[x, y].Neighbors.Add(Cells[xL, yB]);
				Cells[x, y].Neighbors.Add(Cells[x, yB]);
				Cells[x, y].Neighbors.Add(Cells[xR, yB]);
			}
		}
	}
}