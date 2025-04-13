using System.IO;
using System.Text.Json;

namespace cli_life;

public class AppConfig
{
	public int width { get; set; }
	public int height { get; set; }
	public int cellSize { get; set; }
	public double liveDensity { get; set; }
	public char aliveChar { get; set; }
	public char notAliveChar { get; set; }
	public int delay { get; set; }
}

public class CellConfig
{
	public int[] aliveCondition { get; set; }
	public int[] notAliveCondition { get; set; }
}

public class Config
{
	public AppConfig app { get; set; }
	public CellConfig cell { get; set; }
	public static Config Parse(string configPath)
	{
		string jsonString = File.ReadAllText(configPath);
		return JsonSerializer.Deserialize<Config>(jsonString);
	}
}