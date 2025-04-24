using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    var rawParts = GetParts();
    var parts = FormatParts(rawParts);
    foreach (var entry in Patterns)
    {
      int counter = 0;
      foreach (var part in parts)
      {
        if (MatrixComparer.Equivalent(part, entry.Value))
        {
          counter++;
        }
      }
      classification.Add(entry.Key, counter);
    }
    classification.Add("Total parts", parts.Count);
    return classification;
  }
  public int AliveCount()
  {
    int counter = 0;
    foreach (Cell cell in Board.Cells)
    {
      if (cell.IsAlive) counter++;
    }
    return counter;
  }
  private List<int[,]> FormatParts(List<List<Cell>> parts)
  {
    List<int[,]> result = new List<int[,]>();
    foreach (var part in parts)
    {
      var bounds = new Bounds();
      foreach (Cell cell in part)
      {
        bounds.InsertPoint(cell.X, cell.Y);
      }

      int[,] formatted = new int[bounds.Width, bounds.Height];
      foreach (Cell cell in part)
      {
        formatted[cell.X - bounds.MinX, cell.Y - bounds.MinY] = 1;
      }
      result.Add(formatted);
    }
    return result;
  }
  private List<List<Cell>> GetParts()
  {
    var visited = new HashSet<Cell>();
    var parts = new List<List<Cell>>();
    foreach (Cell cell in Board.Cells)
    {
      if (!cell.IsAlive) continue;
      var part = VisitPart(cell, visited);
      if (part.Count != 0) parts.Add(part);
    }
    return parts;
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