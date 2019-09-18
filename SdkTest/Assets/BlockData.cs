using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData
{
	public Cell[] cells = new Cell[9];
	public HashSet<int> missing = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
}
