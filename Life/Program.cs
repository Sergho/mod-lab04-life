using System;
using System.Threading;

namespace cli_life;

class Program
{
    static Board board;
    static AppConfig config;
    static private void Setup()
    {
        config = Config.Parse("config.json").app;
        board = new Board(
            width: config.width,
            height: config.height,
            cellSize: config.cellSize,
            liveDensity: config.liveDensity);
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
                    Console.Write(config.aliveChar);
                }
                else
                {
                    Console.Write(config.notAliveChar);
                }
            }
            Console.Write('\n');
        }
    }
    static void Main(string[] args)
    {
        Setup();
        while (true)
        {
            Console.Clear();
            Render();
            board.Advance();
            Thread.Sleep(config.delay);
        }
    }
}