using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TiledWorld))]
public class TiledWorldInspector : Editor
{
    private IEnumerator mapGenerator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var map = (TiledWorld) target;
        EditorGUILayout.Separator();
        if (mapGenerator == null)
        {
            if (GUILayout.Button("Generate Step by Step"))
            {
                map.Clear();
                mapGenerator = map.GenerateLevelStepByStep();
                if (!mapGenerator.MoveNext())
                    mapGenerator = null;
            }
            else if (GUILayout.Button("Regenerate Level"))
                map.Reset();
        }
        else
        {
            if (GUILayout.Button( "Step Generation" ))
            {
                if (!mapGenerator.MoveNext())
                    mapGenerator = null;
            }
        }

        if (GUILayout.Button("Clear Level"))
        {
            mapGenerator = null;
            map.Clear();
        }
    }
}
