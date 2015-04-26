using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    
    void GeneratePlayer()
    {
        var world = GameObject.FindObjectOfType<TiledWorld>();
        GameObject.Instantiate(PlayerPrefab, world.SamplePosition(), Quaternion.identity);
    }
}
