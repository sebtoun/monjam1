using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    
    void GeneratePlayer()
    {
        if (PlayerPrefab == null)
        { 
            Debug.LogError("Player prefab not set");
            return;
        }

        var world = GameObject.FindObjectOfType<TiledWorld>();

        var box = PlayerPrefab.transform.Find( "Body" ).GetComponent<CircleCollider2D>();
        if (box == null)
        {
            Debug.LogError( "Cannot find Player collider", this );
            return;
        }
        
        Vector2 position;
        if (!world.SampleEmptyPosition(2*box.radius*Vector2.one, out position, iterations: 100))
        {
            Debug.LogError("Could not find room for player !", this);
            return;
        }
        GameObject.Instantiate(PlayerPrefab, position, Quaternion.identity);
    }
}
