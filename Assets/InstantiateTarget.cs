using UnityEngine;
using System.Collections;

public class InstantiateTarget : MonoBehaviour 
{
    void Awake()
    {
        var targetPrefab = Resources.Load<GameObject>( "Target" );
        if (targetPrefab == null)
        {
            Debug.LogError("Could not find Target prefab.", this);
            return;
        }
        GameObject.Instantiate(targetPrefab, transform.position, Quaternion.identity);
    }
}
