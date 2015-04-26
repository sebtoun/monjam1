using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

public class TiledWorld : MonoBehaviour
{
    public int TileWidth = 32;
    public int TileHeight = 32;

    public bool GenerateOnAwake = true;
    
    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject VoidPrefab;

    public Vector3 TileSize = Vector2.one;

    public LevelGenerator Generator;

    public enum TileType
    {
        Wall = 0,
        Floor,
        Void
    }

    public GameObject GetTilePrefab(TileType type)
    {
        switch (type)
        {
            case TileType.Wall:
                return WallPrefab;
            case TileType.Floor:
                return FloorPrefab;
            case TileType.Void:
                return VoidPrefab;
            default:
                throw new ArgumentOutOfRangeException("type");
        }
    }

    // ReSharper disable once UnusedMember.Local
    void Awake()
    {
        if (Generator == null)
            Generator = GetComponent<LevelGenerator>();
        if (Generator == null)
        {
            Debug.LogError("No LevelGenerator attached to this world object.");
            return;            
        }

        if (GenerateOnAwake)
        {
            GenerateLevel();
        }
        
        SendMessage( "GeneratePlayer" );
        SendMessage( "GenerateEnemies" );
    }

    private void GenerateLevel()
    {
        var tiles = new TileType[TileWidth, TileHeight];
        Generator.GenerateLevel(tiles);
        GenerateTiles(tiles);
    }
    
    private void GenerateTiles(TileType[,] tiles)
    {
        for (var x = 0; x < TileHeight; ++x)
            for (var y = 0; y < TileWidth; ++y)
                CreateTile(x, y, tiles[x, y]);
   }

    private void CreateTile(int x, int y, TileType type)
    {
        var position = new Vector3(TileSize.x * x, TileSize.y * y);
        var tile = Instantiate(GetTilePrefab(type), position, Quaternion.identity) as GameObject;

        if (tile == null)
            return;

        tile.transform.parent = transform;
        DontSaveObject(tile);
        tile.name = string.Format("tile_{0}x{1}", x, y);
    }

    private static void DontSaveObject(GameObject obj)
    {
        obj.hideFlags = HideFlags.DontSave;
        foreach (Transform child in obj.transform)
        {
            DontSaveObject(child.gameObject);
        }
    }

    public void Clear()
    {
        var children = (from Transform child
                        in transform
                        select child.gameObject).ToList();
        children.ForEach( DestroyImmediate );
    }

    public void Reset()
    {
        Clear();
        GenerateLevel();
    }

    public Vector2 SamplePosition()
    {
        return new Vector2(Random.value * TileWidth * TileSize.x, Random.value * TileHeight * TileSize.y);
    }
}
