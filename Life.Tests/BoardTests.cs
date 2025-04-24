using cli_life;

namespace Life.Tests;

public class BoardTests
{
    [Fact]
    public void BoardPrimarySizeTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 1);
        Assert.Equal(100, board.Columns);
        Assert.Equal(60, board.Rows);
    }

    [Fact]
    public void BoardComplexSizeTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 2);
        Assert.Equal(50, board.Columns);
        Assert.Equal(30, board.Rows);
    }

    [Fact]
    public void ConnectNeighborsCornerTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 2);
        Assert.Equal(8, board.Cells[0, 0].Neighbors.Count);
    }

    [Fact]
    public void ConnectNeighborsSideTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 2);
        Assert.Equal(8, board.Cells[0, 1].Neighbors.Count);
    }

    [Fact]
    public void ConnectNeighborsInnerTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 60, 1);
        Assert.Equal(8, board.Cells[1, 1].Neighbors.Count);
    }

    [Fact]
    public void CountCellsLimitTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(100, 30, 1);
        Assert.InRange(board.AliveCount, 0, 3000);
    }

    [Fact]
    public void SerializeTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(3, 2, 1, 1);
        Assert.Equal("111\n111\n", board.Serialize());
    }

    [Fact]
    public void DeserializeTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(3, 3, 1, 1);
        board.Deserialize("100\n000\n010");
        Assert.Equal(2, board.AliveCount);
    }

    [Fact]
    public void DeserializeSizeErrorTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(3, 3, 1, 1);
        Assert.Throws<Exception>(() => { board.Deserialize("100\n0000\n010"); });
    }

    [Fact]
    public void ClearTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Board board = new Board(3, 3, 1, 1);
        board.Clear();
        Assert.Equal(0, board.AliveCount);
    }
}
