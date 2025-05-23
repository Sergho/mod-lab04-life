using System;
using System.Collections.Generic;
using System.Linq;

namespace cli_life;

public class Cell
{
	public int X { get; set; }
	public int Y { get; set; }
	public static CellConfig Config;
	public bool IsAlive;
	public readonly List<Cell> Neighbors = new List<Cell>();
	private bool IsAliveNext;
	public Cell(int x, int y)
	{
		X = x;
		Y = y;
	}
	public void DetermineNextLiveState()
	{
		int liveNeighbors = Neighbors.Where(x => x.IsAlive).Count();
		if (IsAlive)
			IsAliveNext = Config.aliveCondition.Contains(liveNeighbors);
		else
			IsAliveNext = Config.notAliveCondition.Contains(liveNeighbors);
	}
	public void Advance()
	{
		IsAlive = IsAliveNext;
	}
}