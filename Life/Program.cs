using System;
using System.IO.Enumeration;
using System.Threading;

namespace cli_life;

class Program
{
    static Board board;
    static Config config;
    static bool paused = false;
    static private void Setup()
    {
        config = Config.Parse("config.json");
        Cell.config = config.cell;
        board = new Board(
            width: config.app.width,
            height: config.app.height,
            cellSize: config.app.cellSize,
            liveDensity: config.app.liveDensity);
    }
    static void Render()
    {
        for (int row = 0; row < board.Rows; row++)
        {
            for (int col = 0; col < board.Columns; col++)
            {
                var cell = board.Cells[col, row];
                if (cell.IsAlive)
                {
                    Console.Write(config.app.aliveChar);
                }
                else
                {
                    Console.Write(config.app.notAliveChar);
                }
            }
            Console.Write('\n');
        }
    }
    static void Main(string[] args)
    {
        Setup();

        Thread mainThread = new Thread(MainPolling);
        Thread keyThread = new Thread(KeyPolling);
        mainThread.Start();
        keyThread.Start();
    }
    static void MainPolling()
    {
        while (true)
        {
            if (paused) continue;
            Console.Clear();
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
                    char ch = key.KeyChar;
                    filename += ch;
                    Console.Write(ch);
                }
                if (key.Key == ConsoleKey.P)
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

            Thread.Sleep(50);
        }
    }
}