using cli_life;

namespace Life.Tests;

public class AnalyzerTests
{
    [Fact]
    public void TotalCountTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 1, 0);
        BoardAnalyzer boardAnalyzer = new BoardAnalyzer(board);
        board.Cells[0, 0].IsAlive = true;
        board.Cells[0, 1].IsAlive = true;
        board.Cells[3, 3].IsAlive = true;
        var classification = boardAnalyzer.GetClassification();
        Assert.Equal(2, classification["Total parts"]);
    }

    [Fact]
    public void ClassifiedCountTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 1, 0);
        BoardAnalyzer boardAnalyzer = new BoardAnalyzer(board);
        board.Cells[0, 0].IsAlive = true;
        board.Cells[0, 1].IsAlive = true;
        board.Cells[0, 2].IsAlive = true;
        var classification = boardAnalyzer.GetClassification();
        Assert.Equal(1, classification["blinker"]);
    }
}
