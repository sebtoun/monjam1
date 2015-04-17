using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ExecutionOrderManager : Editor
{
    static ExecutionOrderManager()
    {
        ExecutionOrderGroups groups = null;
        try
        {
            groups =
                AssetDatabase.LoadAssetAtPath(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ExecutionOrderGroups").First()),
                    typeof (ExecutionOrderGroups)) as ExecutionOrderGroups; 
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
        
        Debug.Log( "Found ExecutionOrderGroups instance: " + groups.GroupsExecutionOrder.ToString() );

        // Get the name of the script we want to change it's execution order
        string scriptName = "";//typeof( MyMonoBehaviourClass ).Name;

        // Iterate through all scripts (Might be a better way to do this?)
        foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
        {
            // If found our script
            if (monoScript.name == scriptName)
            {
                // And it's not at the execution time we want already
                // (Without this we will get stuck in an infinite loop)
                if (MonoImporter.GetExecutionOrder( monoScript ) != -100)
                {
                    MonoImporter.SetExecutionOrder( monoScript, -100 );
                }
                break;
            }
        }
    }
}