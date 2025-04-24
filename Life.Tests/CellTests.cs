using cli_life;

namespace Life.Tests;

public class CellTests
{
    [Fact]
    public void OneAliveNeighborTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Cell cell = new Cell(0, 0) { IsAlive = true };
        cell.Neighbors.AddRange(Enumerable.Repeat(new Cell(0, 0) { IsAlive = true }, 1));
        cell.DetermineNextLiveState();
        cell.Advance();
        Assert.False(cell.IsAlive);
    }

    [Fact]
    public void TwoAliveNeighborsTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Cell cell = new Cell(0, 0) { IsAlive = true };
        cell.Neighbors.AddRange(Enumerable.Repeat(new Cell(0, 0) { IsAlive = true }, 2));
        cell.DetermineNextLiveState();
        cell.Advance();
        Assert.True(cell.IsAlive);
    }

    [Fact]
    public void ThreeAliveNeighborsTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Cell cell = new Cell(0, 0) { IsAlive = true };
        cell.Neighbors.AddRange(Enumerable.Repeat(new Cell(0, 0) { IsAlive = true }, 3));
        cell.DetermineNextLiveState();
        cell.Advance();
        Assert.True(cell.IsAlive);
    }

    [Fact]
    public void FourAliveNeighborsTest()
    {
        Config config = Config.Parse("config.json");
        Cell.Config = config.cell;
        Cell cell = new Cell(0, 0) { IsAlive = true };
        cell.Neighbors.AddRange(Enumerable.Repeat(new Cell(0, 0) { IsAlive = true }, 4));
        cell.DetermineNextLiveState();
        cell.Advance();
        Assert.False(cell.IsAlive);
    }
}
