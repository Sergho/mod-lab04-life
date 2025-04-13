using System;
using System.IO.Enumeration;
using System.Threading;

namespace cli_life;

class Program
{
    static Board board;
    static Config config;
    static bool paused = false;
    static void Main(string[] args)
    {
        config = Config.Parse("config.json");
        Cell.config = config.cell;
        board = new Board(
            width: config.app.width,
            height: config.app.height,
            cellSize: config.app.cellSize,
            liveDensity: config.app.liveDensity);

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
            board.Render(config.app.aliveChar, config.app.notAliveChar);
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