using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
	private Cell[,] grid;
	private Grid instance;

	public void OnEnable()
	{
		instance = ((Grid)target);
		grid = Grid.FillGrid(instance.GetComponentsInChildren<Cell>());
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();
		EditorGUI.BeginChangeCheck();
		for (int i = 0; i < 9; ++i)
		{
			EditorGUILayout.BeginHorizontal();
			for (int j = 0; j < 9; ++j)
			{
				grid[i, j].known = EditorGUILayout.IntField(grid[i, j].known);
			}
			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Clear"))
		{
			for (int i = 0; i < 9; ++i)
			{
				for (int j = 0; j < 9; ++j)
				{
					grid[i, j].known = 0;
				}
			}
		}

		if (EditorGUI.EndChangeCheck())
		{
			var cells = instance.GetComponentsInChildren<Cell>();
			for (int i = 0; i < 9; ++i)
			{
				for (int j = 0; j < 9; ++j)
				{
					var cell = grid[i, j];
					var blockcell = cells[cell.cellBlockID];
					blockcell.known = cell.known;
					EditorUtility.SetDirty(blockcell);
				}
			}
		}
	}
}
