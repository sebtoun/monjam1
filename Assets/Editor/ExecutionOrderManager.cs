using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//[InitializeOnLoad]
public class ExecutionOrderManager : Editor
{
    [MenuItem("Tools/Set Execution Order")]
    public static void SetExecutionOrder()
    {
        Dictionary<string, int> groups;
        try
        {
            var groupsPath =
                AssetDatabase.FindAssets("ExecutionOrderGroups")
                    .Select<string, string>(AssetDatabase.GUIDToAssetPath)
                    .First(path => path.EndsWith(".asset"));
            groups = (AssetDatabase.LoadAssetAtPath(groupsPath, typeof (ExecutionOrderGroups)) as ExecutionOrderGroups).Groups;
        }
        catch (Exception)
        {
            Debug.LogError( "Could not find any ExecutionOrderGroups instance. Create one with menu entry : Assets/Create/ExecutionOrderGroups." );
            return;
        }

        if (groups == null)
        {
            Debug.LogError( "Could not find any ExecutionOrderGroups instance. Create one with menu entry : Assets/Create/ExecutionOrderGroups." );
            return;
        }
        
        foreach (
            MonoScript monoScript in
                MonoImporter.GetAllRuntimeMonoScripts()
                    .Where( script => script.GetClass() != null && Attribute.GetCustomAttribute(script.GetClass(), typeof (ExecutionOrderAttribute)) != null) )
        {
            var executionOrderGroup = (Attribute.GetCustomAttribute(monoScript.GetClass(), typeof (ExecutionOrderAttribute)) as ExecutionOrderAttribute).Group;

            if (!groups.ContainsKey(executionOrderGroup))
            {
                Debug.LogWarning("Unknown execution order group: " + executionOrderGroup);
            }
            else
            {
                var executionOrder = groups[executionOrderGroup];
                if (MonoImporter.GetExecutionOrder( monoScript ) != executionOrder)
                {
                    Debug.Log(string.Format("Setting execution order of {0} to {1}", monoScript.name, executionOrder));
                    MonoImporter.SetExecutionOrder( monoScript, executionOrder);
                }
            }
        }
    }
}