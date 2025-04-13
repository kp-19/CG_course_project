using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (IslandGenerator))]
public class IslandGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
		IslandGenerator islandGen = (IslandGenerator)target;

		if (DrawDefaultInspector ()) {
			if (islandGen.autoUpdate) {
				islandGen.GenerateIsland ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			islandGen.GenerateIsland ();
		}
	}
}
