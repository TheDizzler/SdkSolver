using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
	public int known = 0;
	public HashSet<int> possibles;

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

	public void CheckPossibles(HashSet<int> checkPossibles)
	{
		possibles.Overlaps(checkPossibles);
		if (possibles.Count == 1)
		{
			foreach (int value in possibles)
				known = value;
			text.text = known.ToString();
		}
	}

	public void OnDrawGizmos()
	{
		if (known != 0)
			GetComponentInChildren<Text>().text = known.ToString();
		else
			GetComponentInChildren<Text>().text = "-";
	}
}
