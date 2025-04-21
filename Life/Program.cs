using System;
using System.IO;
using System.Threading;

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
    }
    static void Render()
    {
        Console.Clear();
        string render = board.Render(config.app.aliveChar, config.app.notAliveChar);
        Console.Write(render);
        Console.WriteLine(boardAnalyzer.GetPartsCount());
    }
    static void MainPolling()
    {
        while (true)
        {
            if (paused) continue;
            Render();
            board.Advance();
            Thread.Sleep(config.app.delay);
        }
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
    static string GetPath(string filename)
    {
        return "saves/" + filename + ".txt";
    }
    static void SaveFile(string filename)
    {
        Directory.CreateDirectory("saves");
        File.WriteAllText(GetPath(filename), board.Serialize());
    }
    static string ReadFile(string filename)
    {
        string path = GetPath(filename);
        if (!File.Exists(path)) throw new Exception("File not found");
        return File.ReadAllText(path);
    }
}