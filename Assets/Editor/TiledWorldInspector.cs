using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TiledWorld))]
public class TiledWorldInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var map = (TiledWorld) target;
        EditorGUILayout.Separator();

        if (GUILayout.Button("Regenerate Level"))
            map.Reset();
    }
}
