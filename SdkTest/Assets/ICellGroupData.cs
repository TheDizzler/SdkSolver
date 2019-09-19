using System.Collections.Generic;
using UnityEngine;

public abstract class ICellGroupData
{
	public Cell[] cells = new Cell[9];
	public HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

	public bool FindMissing()
	{
		bool changesMade = false;
		foreach (Cell cell in cells)
		{
			if (cell.known != 0)
			{
				missing.Remove(cell.known);
			}
		}

		foreach (Cell cell in cells)
		{
			if (cell.known == 0)
			{
				if (cell.CheckPossibles(missing))
					changesMade = true;
			}
		}

		return changesMade;
	}

	public bool FindUnique()
	{
		bool notifyChanges = false;
		bool changesMade = true;
		while (changesMade)
		{
			changesMade = false;
			foreach (Cell cell in cells)
			{
				if (cell.known != 0)
					continue;
				HashSet<int> possibles = cell.GetPossibles();

				foreach (Cell otherCell in cells)
				{
					if (otherCell.known != 0 || otherCell == cell)
						continue;
					possibles.ExceptWith(otherCell.GetPossibles());
				}

				if (possibles.Count == 1)
				{
					foreach (int value in possibles)
						cell.SetKnown(value);
					missing.Remove(cell.known);
					changesMade = true;
					notifyChanges = true;
				}
			}
		}

		return notifyChanges;
	}

	public bool FindDoubles()
	{
		bool changesMade = false;
		foreach (Cell cell in cells)
		{
			if (cell.GetPossibles().Count != 2 || cell == cells[8])
				continue;

			foreach (Cell otherCell in cells)
			{
				if (otherCell.GetPossibles().Count != 2 || cell == otherCell)
					continue;

				HashSet<int> possibles = cell.GetPossibles();
				possibles.IntersectWith(otherCell.GetPossibles());
				if (possibles.Count == 2)
				{
					string values = "";
					foreach (int value in possibles)
						values += value.ToString() + ",";
					//Debug.Log("may have match with " + values + " in cell " + cell.cellBlockID);
					foreach (Cell anotherCell in cells)
					{
						if (anotherCell.known != 0 || cell == anotherCell || otherCell == anotherCell)
							continue;
						if (anotherCell.Remove(possibles))
						{
							Debug.Log(values + " removed!");
							changesMade = true;
						}
					}
				}
			}
		}

		return changesMade;
	}

	public bool FindHarderDoubles()
	{
		bool changesMade = false;
		Cell dontRemove = null;
		foreach (Cell cell in cells)
		{
			if (cell.GetPossibles().Count != 2)
				continue;

			bool foundOneSubset = false;
			HashSet<int> possibles = cell.GetPossibles();
			string values = "";
			foreach (Cell otherCell in cells)
			{
				if (otherCell.known != 0 || cell == otherCell)
					continue;


				if (possibles.IsSubsetOf(otherCell.GetPossibles()))
				{
					values = "";
					foreach (int value in possibles)
						values += value.ToString() + ",";
					Debug.Log("Found a subset with " + values + " in cell " + cell.cellBlockID);
					if (foundOneSubset)
					{
						Debug.Log("Bunk...this is useless");
						foundOneSubset = false;
						break;
					}
					else
					{
						dontRemove = otherCell;
						foundOneSubset = true;
					}
				}
			}

			if (foundOneSubset)
			{
				foreach (Cell anotherCell in cells)
				{
					if (anotherCell.known != 0 || cell == anotherCell || dontRemove == anotherCell)
						continue;
					if (anotherCell.Remove(possibles))
					{
						Debug.Log(values + " removed!");
						changesMade = true;
					}
				}
			}
		}

		return changesMade;
	}


	//private bool CheckForUniqueSet(Cell cell, Cell otherCell, HashSet<int> possibles)
	//{
	//	foreach (Cell anotherCell in cells)
	//	{
	//		if (anotherCell.known != 0 || cell == anotherCell || otherCell == anotherCell)
	//			continue;

	//		possibles.IntersectWith(anotherCell.GetPossibles());
	//		if (possibles.Count == 2)
	//		{
	//			Debug.Log(" we was wrong :(");
	//			return false;
	//		}
	//	}

	//	return true;
	//}
}
