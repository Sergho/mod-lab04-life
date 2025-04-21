using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cli_life;

class BoardAnalyzer
{
  public Board Board { get; set; }
  public BoardAnalyzer(Board board)
  {
    Board = board;
  }
  public int GetPartsCount()
  {
    var visited = new HashSet<Cell>();
    int counter = 0;
    foreach (Cell cell in Board.Cells)
    {
      if (cell.IsAlive && !IsVisitedPart(cell, visited)) counter++;
    }
    return counter;
  }
  private bool IsVisitedPart(Cell cell, HashSet<Cell> visited)
  {
    if (visited.Contains(cell)) return true;
    var queue = new Queue<Cell>();
    queue.Enqueue(cell);

    while (queue.Count != 0)
    {
      Cell current = queue.Dequeue();

      foreach (Cell neighbor in current.Neighbors)
      {
        if (neighbor.IsAlive && !visited.Contains(neighbor))
        {
          visited.Add(neighbor);
          queue.Enqueue(neighbor);
        }
      }
    }
    return false;
  }
}