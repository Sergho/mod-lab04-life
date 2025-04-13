using System;
using System.Threading;

namespace cli_life;

class Program
{
    static Board board;
    static Config config;
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
        while (true)
        {
            Console.Clear();
            Render();
            board.Advance();
            Thread.Sleep(config.app.delay);
        }
    }
}