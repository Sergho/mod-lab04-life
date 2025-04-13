using System;
using System.Collections.Generic;
using System.Linq;

namespace cli_life;

public class Cell
{
	public static CellConfig config;
	public bool IsAlive;
	public readonly List<Cell> neighbors = new List<Cell>();
	private bool IsAliveNext;
	public void DetermineNextLiveState()
	{
		int liveNeighbors = neighbors.Where(x => x.IsAlive).Count();
		if (IsAlive)
			IsAliveNext = config.aliveCondition.Contains(liveNeighbors);
		else
			IsAliveNext = config.notAliveCondition.Contains(liveNeighbors);
	}
	public void Advance()
	{
		IsAlive = IsAliveNext;
	}
}