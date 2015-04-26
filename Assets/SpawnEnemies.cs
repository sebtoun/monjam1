﻿using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int InitialAmount = 40;

    void GenerateEnemies()
    {
        var world = GameObject.FindObjectOfType<TiledWorld>();
        var root = transform.Find("/Enemies");
        if (root == null)
        {
            root = new GameObject("Enemies").transform;
        }
        
        var box = enemyPrefab.transform.Find("Body").GetComponent<BoxCollider2D>();
        if (box == null)
        {
            Debug.LogError("Cannot find box collider");
        }
        var mask = (1 << LayerMask.NameToLayer("Tiles")) | (1 << LayerMask.NameToLayer("Default"));

        for (int i = 0; i < InitialAmount; i++)
        {
            var position = Vector2.zero;
            int iterations = 5;
            do
            {
                position = world.SamplePosition();
                iterations--;
            } while (iterations > 0 && Physics2D.OverlapArea(position + box.center - box.size/2, position + box.center + box.size/2, mask) != null );

            if (iterations == 0)
                Debug.LogWarning("Could not instantiate enemy " + i);
            else
            {
                var newEnemy = GameObject.Instantiate(enemyPrefab, position, Quaternion.AngleAxis(Random.value*360, Vector3.forward)) as GameObject;
                newEnemy.transform.parent = root;
            }
        }
    }
}