namespace cli_life;

public class Bounds
{
	public int MinX { get; set; }
	public int MaxX { get; set; }
	public int MinY { get; set; }
	public int MaxY { get; set; }
	public int Width => MaxX - MinX + 1;
	public int Height => MaxY - MinY + 1;
	public Bounds()
	{
		MinX = -1;
		MaxX = -1;
		MinY = -1;
		MaxY = -1;
	}
	public void InsertPoint(int x, int y)
	{
		if (MinX == -1 || x < MinX) MinX = x;
		if (MaxX == -1 || x > MaxX) MaxX = x;
		if (MinY == -1 || y < MinY) MinY = y;
		if (MaxY == -1 || y > MaxY) MaxY = y;
	}
}