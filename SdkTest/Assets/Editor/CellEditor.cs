using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
	private Cell cell;


	public void OnEnable()
	{
		cell = (Cell) target;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUI.enabled = false;
		EditorGUILayout.BeginHorizontal();
		foreach (int value in cell.GetPossibles())
		{
			EditorGUILayout.IntField(value);
		}
		EditorGUILayout.EndHorizontal();
		GUI.enabled = true;
	}
}
