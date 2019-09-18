using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
	public int known = 0;
	public HashSet<int> possibles;
	public int cellBlockID;

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

	public IEnumerable<int> GetPossibles()
	{
		if (known != 0)
		{
			return new HashSet<int> { known };
		}
		else
		{
			if (possibles == null)
				return new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			return possibles;
		}
	}

	/// <summary>
	/// 
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
			text.text = known.ToString();
			return true;
		}

		return false;
	}

	public void OnDrawGizmos()
	{
		if (known != 0)
			GetComponentInChildren<Text>().text = known.ToString();
		else
			GetComponentInChildren<Text>().text = "-";
	}
}
