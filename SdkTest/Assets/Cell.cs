using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
	public int known = 0;
	public int cellBlockID;

	public RowData rowData;
	public ColData colData;
	public BlockData blockData;

	private HashSet<int> possibles;
	private Text text;


	public void Start()
	{
		text = GetComponentInChildren<Text>();
		if (known != 0)
		{
			text.text = known.ToString();
			possibles = new HashSet<int> { known };
		}
		else
		{
			text.text = "-";
			possibles = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}
	}

	/// <summary>
	/// creates copy
	/// </summary>
	/// <returns></returns>
	public HashSet<int> GetPossibles()
	{
		if (known != 0)
		{
			return new HashSet<int> { known };
		}
		else
		{
			if (possibles == null)
				return new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			HashSet<int> copy = new HashSet<int>();
			foreach (int value in possibles)
				copy.Add(value);
			return copy;
		}
	}

	public void SetKnown(int newKnown)
	{
		known = newKnown;
		possibles = new HashSet<int> { known };
		text.color = Color.green;
		text.text = known.ToString();
	}

	/// <summary>
	/// true if cell became known
	/// </summary>
	/// <param name="possibilites">true if cell became known</param>
	/// <returns></returns>
	public bool CheckPossibles(HashSet<int> possibilites)
	{
		possibles.IntersectWith(possibilites);
		if (possibles.Count == 1)
		{
			foreach (int value in possibles)
				known = value;
			text.color = Color.gray;
			text.text = known.ToString();
			return true;
		}
		else if (known == 0 && possibles.Count < 6)
		{
			if (possibles.Count == 0)
			{
				Debug.LogError("BLAARGH " + cellBlockID);
				text.color = Color.yellow;
				text.text = "!";
			}

			text.color = Color.red;
			text.text = ICellGroupData.GetValues(possibles);
		}

		return false;
	}

	/// <summary>
	/// Returns true when a change made
	/// </summary>
	/// <param name="remove"></param>
	/// <returns></returns>
	public bool Remove(HashSet<int> remove)
	{
		int precount = possibles.Count;
		possibles.ExceptWith(remove);

		if (possibles.Count == 1 && precount != 1)
		{
			foreach (int value in possibles)
				known = value;
			text.color = Color.cyan;
			text.text = known.ToString();
			return true;
		}

		if (precount != possibles.Count)
		{
			if (possibles.Count == 0)
			{
				Debug.LogError("Major ufkcup in cell block " + cellBlockID);
				text.color = Color.yellow;
				text.text = "!";
			}

			return true;
		}

		return false;
	}


	public void OnDrawGizmos()
	{
		if (known != 0)
			GetComponentInChildren<Text>().text = known.ToString();
		else if (possibles == null)
			GetComponentInChildren<Text>().text = "-";
	}
}