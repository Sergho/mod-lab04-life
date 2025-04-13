using System;
using System.Threading;

namespace cli_life;

class Program
{
    static Board board;
    static private void Reset()
    {
        board = new Board(
            width: 50,
            height: 20,
            cellSize: 1,
            liveDensity: 0.5);
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
                    Console.Write("*");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.Write('\n');
        }
    }
    static void Main(string[] args)
    {
        Reset();
        while (true)
        {
            Console.Clear();
            Render();
            board.Advance();
            Thread.Sleep(1000);
        }
    }
}