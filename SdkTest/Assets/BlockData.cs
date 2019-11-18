using System.Collections.Generic;
using UnityEngine;

public class BlockData : ICellGroupData
{
	/// <summary>
	/// Find a row with ONLY three possibles. If this is true it can be asserted
	/// that those three numbers are NOT possible in the other rows of this block
	/// NOR the cells in other blocks of that row.
	/// </summary>
	public bool FindTripletRowInBlock()
	{
		bool changesMade = false;
		for (int row = 0; row < 3; ++row)
		{
			if (cells[row * 3].known != 0
				|| cells[row * 3 + 1].known != 0
				|| cells[row * 3 + 2].known != 0)
			{
				continue;
			}

			HashSet<int> checkTriplet = new HashSet<int>();
			checkTriplet.UnionWith(cells[row * 3].GetPossibles());
			checkTriplet.UnionWith(cells[row * 3 + 1].GetPossibles());
			checkTriplet.UnionWith(cells[row * 3 + 2].GetPossibles());

			if (checkTriplet.Count == 3)
			{
				string values = GetValues(checkTriplet);

				// remove numbers from other rows in this block
				foreach (Cell cell in cells)
				{
					if (cell == cells[row * 3]
						|| cell == cells[row * 3 + 1]
						|| cell == cells[row * 3 + 2]
						|| cell.known != 0)
					{
						continue;
					}

					if (cell.Remove(checkTriplet))
						changesMade = true;
				}

				// remove numbers from other blocks on this row
				foreach (Cell rowCell in cells[row * 3].rowData.cells)
				{
					if (rowCell == cells[row * 3]
						|| rowCell == cells[row * 3 + 1]
						|| rowCell == cells[row * 3 + 2]
						|| rowCell.known != 0)
					{
						continue;
					}

					if (rowCell.Remove(checkTriplet))
						changesMade = true;
				}

				if (changesMade)
					Debug.Log("Triple town! " + values + " in cell row " + cells[row * 3].cellBlockID);
			}
		}

		return changesMade;
	}

	/// <summary>
	/// Find a col with ONLY three possibles. If this is true it can be asserted
	/// that those three numbers are NOT possible in the other cols of this block.
	/// </summary>
	public bool FindTripletColInBlock()
	{
		bool changesMade = false;
		for (int col = 0; col < 3; ++col)
		{
			if (cells[col].known != 0
				|| cells[col + 3].known != 0
				|| cells[col + 6].known != 0)
			{
				continue;
			}

			HashSet<int> checkTriplet = new HashSet<int>();
			checkTriplet.UnionWith(cells[col].GetPossibles());
			checkTriplet.UnionWith(cells[col + 3].GetPossibles());
			checkTriplet.UnionWith(cells[col + 6].GetPossibles());

			if (checkTriplet.Count == 3)
			{
				string values = GetValues(checkTriplet);
				// remove numbers from other cols in this block
				foreach (Cell cell in cells)
				{
					if (cell == cells[col]
						|| cell == cells[col + 3]
						|| cell == cells[col + 6]
						|| cell.known != 0)
					{
						continue;
					}

					if (cell.Remove(checkTriplet))
						changesMade = true;
				}

				// remove numbers from other blocks on this row
				foreach (Cell colCell in cells[col].colData.cells)
				{
					if (colCell == cells[col]
						|| colCell == cells[col + 3]
						|| colCell == cells[col + 6]
						|| colCell.known != 0)
					{
						continue;
					}

					if (colCell.Remove(checkTriplet))
						changesMade = true;
				}

				if (changesMade)
					Debug.Log("Triple town! " + values + " in cell col " + cells[col].cellBlockID);
			}

		}

		return changesMade;
	}
}
