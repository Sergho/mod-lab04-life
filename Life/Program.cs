using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Palettes;

namespace cli_life;

class Program
{
    static BoardAnalyzer boardAnalyzer;
    static Board board;
    static Config config;
    static bool paused = true;
    static void Main(string[] args)
    {
        config = Config.Parse("config.json");
        Cell.Config = config.cell;
        if (args.Length > 0 && args[0] == "--report") ReportStart();
        else GameStart();
    }
    static void ReportStart()
    {
        Directory.CreateDirectory("report");

        Plot plot = new Plot();
        Random rand = new Random();
        for (double density = 0; density <= 1; density += config.report.densityStep)
        {
            Console.WriteLine($"Density: {Math.Round(density, 3)}");
            File.WriteAllText($"report/density-{Math.Round(density, 3)}.txt", "");

            board = new Board(
                width: config.app.width,
                height: config.app.height,
                cellSize: config.app.cellSize,
                liveDensity: density);

            List<int> generationsList = new List<int>();
            List<int> aliveCountList = new List<int>();

            while (board.StableCount < config.app.exitCondition)
            {
                File.AppendAllText($"report/density-{Math.Round(density, 3)}.txt", $"{board.Generation} {board.AliveCount}\n");
                generationsList.Add(board.Generation);
                aliveCountList.Add(board.AliveCount);
                board.Advance();
            }

            var sig = plot.Add.Scatter(generationsList, aliveCountList);
            sig.MarkerSize = 1;
            sig.LegendText = $"Density {Math.Round(density, 3)}";
        }

        plot.Title("Report Graph", size: 16);
        plot.XLabel("Generation", size: 14);
        plot.YLabel("Alive cells", size: 14);

        plot.Axes.AutoScale();
        plot.ShowLegend();

        plot.SavePng("report/plot.png", 2000, 1000);
    }
    static void GameStart()
    {
        board = new Board(
            width: config.app.width,
            height: config.app.height,
            cellSize: config.app.cellSize,
            liveDensity: config.app.liveDensity);
        boardAnalyzer = new BoardAnalyzer(board);

        Console.Write("Load from: ");
        string filename = Console.ReadLine();
        if (filename != "")
        {
            string content = ReadFile(filename);
            board.Deserialize(content);
        }

        Render();

        Thread mainThread = new Thread(MainPolling);
        Thread keyThread = new Thread(KeyPolling);
        mainThread.Start();
        keyThread.Start();

        keyThread.Join();
    }
    static void Render()
    {
        Console.Clear();
        string render = board.Render(config.app.aliveChar, config.app.notAliveChar);
        Console.Write(render);
        var classification = boardAnalyzer.GetClassification();
        foreach (var entry in classification)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
        Console.WriteLine($"Alive cells: {board.AliveCount}");
        Console.WriteLine($"Generation: {board.Generation}");
    }
    static void MainPolling()
    {
        while (true)
        {
            if (paused) continue;
            if (board.StableCount >= config.app.exitCondition) break;
            Render();
            board.Advance();
            Thread.Sleep(config.app.delay);
        }
        Environment.Exit(0);
    }
    static void KeyPolling()
    {
        string filename = "";
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (paused)
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        SaveFile(filename);
                        filename = "";
                        paused = false;
                        continue;
                    }
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        filename = filename.Remove(filename.Length - 1);
                        Console.Write("\b \b");
                        continue;
                    }
                    char ch = key.KeyChar;
                    filename += ch;
                    Console.Write(ch);
                }
                if (!paused && key.Key == ConsoleKey.P)
                {
                    Console.Write("Save as: ");
                    paused = true;
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    filename = "";
                    paused = false;
                }
            }

            Thread.Sleep(config.app.delay);
        }
    }
    static string GetSavesPath(string filename)
    {
        return "saves/" + filename + ".txt";
    }
    static void SaveFile(string filename)
    {
        Directory.CreateDirectory("saves");
        File.WriteAllText(GetSavesPath(filename), board.Serialize());
    }
    static string ReadFile(string filename)
    {
        string path = GetSavesPath(filename);
        if (!File.Exists(path)) throw new Exception("File not found");
        return File.ReadAllText(path);
    }
}