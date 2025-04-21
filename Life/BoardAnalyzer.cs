using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace cli_life;

class BoardAnalyzer
{
  public Board Board { get; set; }
  public Dictionary<string, int[,]> Patterns { get; set; }
  public BoardAnalyzer(Board board)
  {
    Board = board;
    Patterns = GetPatterns("saves/examples");
  }
  public Dictionary<string, int> GetClassification()
  {
    var classification = new Dictionary<string, int>();
    foreach (var entry in Patterns)
    {
      int count = GetPartsCount(entry.Value);
      classification.Add(entry.Key, count);
    }
    return classification;
  }
  private int GetPartsCount(int[,] pattern)
  {
    var visited = new HashSet<Cell>();
    var parts = new List<List<Cell>>();
    foreach (Cell cell in Board.Cells)
    {
      if (!cell.IsAlive) continue;
      var part = VisitPart(cell, visited);
      if (part.Count != 0) parts.Add(part);
    }
    return parts.Count;
  }
  private List<Cell> VisitPart(Cell cell, HashSet<Cell> visited)
  {
    if (visited.Contains(cell)) return new List<Cell>();
    var queue = new Queue<Cell>();
    var list = new List<Cell>();
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
          list.Add(neighbor);
        }
      }
    }
    return list;
  }
  private Dictionary<string, int[,]> GetPatterns(string prefix)
  {
    if (!Directory.Exists(prefix)) return new Dictionary<string, int[,]>();
    string[] filenames = Directory.GetFiles(prefix);
    Dictionary<string, int[,]> patterns = new Dictionary<string, int[,]>();
    foreach (var filename in filenames)
    {
      string[] content = File.ReadAllLines(filename);
      int[,] pattern = new int[content.Length, content[0].Length];

      for (int i = 0; i < pattern.GetLength(0); i++)
      {
        for (int j = 0; j < pattern.GetLength(1); j++)
        {
          if (content[i][j] == '1') pattern[i, j] = 1;
          else pattern[i, j] = 0;
        }
      }

      patterns.Add(Path.GetFileNameWithoutExtension(filename), pattern);
    }
    return patterns;
  }
}