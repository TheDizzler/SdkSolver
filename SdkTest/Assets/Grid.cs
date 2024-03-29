﻿using UnityEngine;

public class Grid : MonoBehaviour
{
	public Cell[,] grid = new Cell[9, 9];
	public bool test;

	private bool allDone;

	public BlockData[] blocks = new BlockData[9];
	public RowData[] rows = new RowData[9];
	public ColData[] cols = new ColData[9];


	public static Cell[,] FillGrid(Cell[] cells)
	{
		Cell[,] grid = new Cell[9, 9];
		int celli = 0;
		int cellj = 0;
		int blockj = 0;
		int blocki = 0;
		for (int i = 0; i < 9; ++i)
		{
			for (int j = 0; j < 9; ++j)
			{
				int cellNum = blockj * 9 + blocki * 27 + cellj + celli * 3;
				cells[cellNum].cellBlockID = cellNum;
				grid[i, j] = cells[cellNum];
				if (++cellj > 2)
				{
					cellj = 0;
					if (++blockj > 2)
					{
						blockj = 0;
						if (++celli > 2)
						{
							celli = 0;
							++blocki;
						}
					}
				}
			}
		}

		return grid;
	}


	public void Start()
	{
		grid = FillGrid(GetComponentsInChildren<Cell>());


		for (int i = 0; i < 9; ++i)
		{
			rows[i] = new RowData();
			for (int j = 0; j < 9; ++j)
			{
				rows[i].cells[j] = grid[i, j];


				if (cols[j] == null)
					cols[j] = new ColData();

				cols[j].cells[i] = grid[i, j];

				grid[i, j].rowData = rows[i];
				grid[i, j].colData = cols[j];
			}
		}

		int blockNum = 0;
		for (int blocki = 0; blocki < 3; ++blocki)
		{
			for (int blockj = 0; blockj < 3; ++blockj)
			{
				blocks[blockNum] = new BlockData();
				int cellNum = 0;
				for (int i = 0; i < 3; ++i)
				{
					for (int j = 0; j < 3; ++j)
					{
						blocks[blockNum].cells[cellNum++] = grid[blocki * 3 + i, blockj * 3 + j];
						grid[blocki * 3 + i, blockj * 3 + j].blockData = blocks[blockNum];
					}
				}

				++blockNum;
			}
		}
	}


	public void FirstPass()
	{
		bool changesMade = true;
		while (changesMade)
		{
			changesMade = false;
			for (int i = 0; i < 9; ++i)
			{
				if (blocks[i].FindMissing()
					|| rows[i].FindMissing()
					|| cols[i].FindMissing())
					changesMade = true;
			}
		}
	}

	public void FindUniques()
	{
		bool changesMade = false;
		for (int i = 0; i < 9; ++i)
		{
			if (blocks[i].FindUnique())
				changesMade = true;
		}

		if (changesMade)
		{
			FirstPass();
			changesMade = false;
		}

		for (int i = 0; i < 9; ++i)
		{
			if (rows[i].FindUnique())
				changesMade = true;
		}

		if (changesMade)
		{
			FirstPass();
			changesMade = false;
		}

		for (int i = 0; i < 9; ++i)
		{
			if (cols[i].FindUnique())
				changesMade = true;
		}

		if (changesMade)
		{
			FirstPass();
			changesMade = false;
		}
	}


	public void FindUniquesBlocks()
	{
		bool changesMade = false;
		for (int i = 0; i < 9; ++i)
		{
			if (blocks[i].FindUnique())
				changesMade = true;
		}

		if (changesMade)
			FirstPass();
	}

	public void FindUniquesRows()
	{
		for (int i = 0; i < 9; ++i)
		{
			rows[i].FindUnique();
		}
	}

	public void FindUniquesCols()
	{
		for (int i = 0; i < 9; ++i)
		{
			cols[i].FindUnique();
		}
	}

	public void FindDoubles()
	{
		bool again = true;
		while (again)
		{
			again = false;
			bool changesMade = false;
			for (int i = 0; i < 9; ++i)
			{
				if (cols[i].FindDoubles())
					changesMade = true;
			}

			if (changesMade)
			{
				FirstPass();
				changesMade = false;
				again = true;
			}

			for (int i = 0; i < 9; ++i)
			{
				if (rows[i].FindDoubles())
					changesMade = true;
			}

			if (changesMade)
			{
				FirstPass();
				changesMade = false;
				again = true;
			}

			for (int i = 0; i < 9; ++i)
			{
				if (blocks[i].FindDoubles())
					changesMade = true;
			}

			if (changesMade)
			{
				FirstPass();
				changesMade = false;
				again = true;
			}
		}
	}


	public void FindTripletsInBlock()
	{
		bool changesMade = true;
		while (changesMade)
		{
			changesMade = false;
			for (int i = 0; i < 9; ++i)
			{
				if (blocks[i].FindTripletRowInBlock())
					changesMade = true;
			}

			if (changesMade)
			{
				FirstPass();
				continue;
			}

			for (int i = 0; i < 9; ++i)
			{
				if (blocks[i].FindTripletColInBlock())
					changesMade = true;
			}

			if (changesMade)
			{
				FirstPass();
			}
		}
	}

	//public void SimpleCheck()
	//{
	//	bool changesMade = true;
	//	allDone = true;
	//	while (changesMade)
	//	{
	//		changesMade = false;
	//		for (int i = 0; i < 9; ++i)
	//		{
	//			for (int j = 0; j < 9; ++j)
	//			{
	//				CheckBlock(i, j);
	//				CheckRow(i, j);
	//				CheckCol(i, j);
	//			}
	//		}
	//	}

	//	if (allDone)
	//		Debug.Log("Completed!");
	//}


	//private void CheckCol(int i, int j)
	//{
	//	Cell current = grid[i, j];
	//	if (current.known != 0)
	//		return;

	//	allDone = false;
	//	HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

	//	for (int row = 0; row < 9; ++row)
	//	{
	//		if (grid[row, j].known != 0)
	//			if (!missing.Remove(grid[row, j].known))
	//				Debug.LogError(grid[row, j].known + " could not be removed from block");
	//	}

	//	if (missing.Count < 9)
	//	{
	//		if (current.CheckPossibles(missing))
	//			changesMade = true;
	//	}
	//}

	//private void CheckRow(int i, int j)
	//{
	//	Cell current = grid[i, j];
	//	if (current.known != 0)
	//		return;

	//	allDone = false;
	//	HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

	//	for (int col = 0; col < 9; ++col)
	//	{
	//		if (grid[i, col].known != 0)
	//			if (!missing.Remove(grid[i, col].known))
	//				Debug.LogError(grid[i, col].known + " could not be removed from block");
	//	}

	//	if (missing.Count < 9)
	//	{
	//		if (current.CheckPossibles(missing))
	//			changesMade = true;
	//	}
	//}

	//private void CheckBlock(int i, int j)
	//{
	//	Cell current = grid[i, j];
	//	if (current.known != 0)
	//		return;

	//	allDone = false;
	//	HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // get block

	//	int startRow = Mathf.FloorToInt(i / 3) * 3;
	//	int startCol = Mathf.FloorToInt(j / 3) * 3;
	//	for (int row = startRow; row < startRow + 3; ++row)
	//	{
	//		for (int col = startCol; col < startCol + 3; ++col)
	//		{
	//			if (grid[row, col].known != 0)
	//				if (!missing.Remove(grid[row, col].known))
	//					Debug.LogError(grid[row, col].known + " could not be removed from block");
	//		}
	//	}

	//	if (missing.Count < 9)
	//		if (current.CheckPossibles(missing))
	//			changesMade = true;
	//}
}