using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public int InitialAmount = 40;

    void GenerateEnemies()
    {
        var world = GameObject.FindObjectOfType<TiledWorld>();
        var root = transform.Find("/Enemies");
        if (root == null)
        {
            root = new GameObject("Enemies").transform;
        }
        
        var box = EnemyPrefab.transform.Find("Body").GetComponent<CircleCollider2D>();
        if (box == null)
        {
            Debug.LogError("Cannot find box collider");
        }
        
        for (int i = 0; i < InitialAmount; i++)
        {
            var position = Vector2.zero;
            if (!world.SampleEmptyPosition( 2 * box.radius * Vector2.one, out position))
                Debug.LogWarning("Could not instantiate enemy " + i);
            else
            {
                var newEnemy = GameObject.Instantiate(EnemyPrefab, position, Quaternion.AngleAxis(Random.value*360, Vector3.forward)) as GameObject;
                newEnemy.transform.parent = root;
            }
        }
    }
}
