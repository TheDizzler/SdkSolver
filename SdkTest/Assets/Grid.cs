using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public Cell[,] grid = new Cell[9, 9];


	public void Start()
	{
		var cells = GetComponentsInChildren<Cell>();
		int i = 0;
		int j = 0;
		int subi = 0;
		int subj = 0;
		int blocki = 0;
		int blockj = 0;
		foreach (Cell cell in cells)
		{
			cell.name = "" + (j * 8 + (i + j));
			grid[i++, j] = cell;
			if (++subi >= 3)
			{
				subi = 0;
				i = 0;
				if (++j >= 3)
				{
					++blocki;
				}
			}

			//cell.name = "" + (j * 8 + (i + j));
			//grid[i++, j] = cell;
			//if (i >= 9)
			//{
			//	i = 0;
			//	++j;
			//}
		}
	}


	public void FirstPass()
	{
		for (int i = 0; i < 9; ++i)
		{
			for (int j = 0; j < 9; ++j)
			{
				CheckBlock(i, j);
			}
		}
	}

	private void CheckBlock(int i, int j)
	{
		Cell current = grid[i, j];
		if (current.known != 0)
			return;

		HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };


		int startRow = (i / 3) * 3;
		int startCol = (j / 3) * 3;
		for (int row = startRow; row < startRow + 3; ++row)
		{
			for (int col = startCol; col < startCol + 3; ++col)
			{
				if (grid[row, col].known != 0)
					if (!missing.Remove(grid[row, col].known))
						Debug.LogError(grid[row, col].known + " could not be removed from block");
			}
		}

		current.CheckPossibles(missing);
	}
}
