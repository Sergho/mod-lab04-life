namespace cli_life;

public static class MatrixComparer
{
	public static int[,] Rotate90(int[,] matrix)
	{
		int[,] rotated = new int[matrix.GetLength(1), matrix.GetLength(0)];
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				rotated[j, matrix.GetLength(0) - 1 - i] = matrix[i, j];
			}
		}
		return rotated;
	}
	public static int[,] Mirror(int[,] matrix)
	{
		int[,] mirrored = new int[matrix.GetLength(0), matrix.GetLength(1)];
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				mirrored[i, matrix.GetLength(1) - 1 - j] = matrix[i, j];
			}
		}
		return mirrored;
	}
	public static bool Equal(int[,] first, int[,] second)
	{
		if (first.GetLength(0) != second.GetLength(0) || first.GetLength(1) != second.GetLength(1))
			return false;

		for (int i = 0; i < first.GetLength(0); i++)
			for (int j = 0; j < first.GetLength(1); j++)
				if (first[i, j] != second[i, j])
					return false;
		return true;
	}
	public static bool Equivalent(int[,] first, int[,] second)
	{
		if (!Equal(first, second))
		{
			// Проверяем все повороты (0°, 90°, 180°, 270°)
			int[,] rotated = first;
			for (int i = 0; i < 3; i++)
			{
				rotated = Rotate90(rotated);
				if (Equal(rotated, second)) return true;
			}

			rotated = Mirror(first);
			for (int i = 0; i < 3; i++)
			{
				rotated = Rotate90(rotated);
				if (Equal(rotated, second)) return true;
			}

			return false;
		}
		return true;
	}
}