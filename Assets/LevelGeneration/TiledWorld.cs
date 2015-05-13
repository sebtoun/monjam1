using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TiledWorld : MonoBehaviour
{
    public bool GenerateOnAwake = true;
    
    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject VoidPrefab;

    public Vector3 TileSize = Vector2.one;

    public LevelGenerator Generator;

    private TileType[,] _tiles;

    public int TileWidth
    {
        get { return _tiles.GetLength(0); }
    }
    public int TileHeight
    {
        get { return _tiles.GetLength(1); }
    }
    
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

    public void GenerateLevel()
    {
        _tiles = Generator.GenerateLevel();
        GenerateTiles(_tiles);
    }

    public IEnumerator GenerateLevelStepByStep()
    {
        var levelGen = Generator.GenerateLevelStepByStep();

        while (levelGen.MoveNext())
        {
            _tiles = levelGen.Current as TileType[,];
            Clear();
            GenerateTiles( _tiles );
            yield return null;
        }
        Clear();
        GenerateTiles( _tiles );
    }
    
    private void GenerateTiles(TileType[,] tiles)
    {
        for (var x = 0; x < TileHeight; ++x)
            for (var y = 0; y < TileWidth; ++y)
                CreateTile(x, y, tiles);
    }

    private void SetWallTile(int x, int y, TileType[,] tiles, GameObject tile)
    {
        var offset = 0;
        var neighbours = 0;
        if (y < TileHeight - 1 && tiles[x, y + 1] == TileType.Floor) neighbours |= 1;
        if (x < TileWidth - 1 && tiles[x + 1, y] == TileType.Floor) neighbours |= 2;
        if (y > 0 && tiles[x, y - 1] == TileType.Floor) neighbours |= 4;
        if (x > 0 && tiles[x - 1, y] == TileType.Floor) neighbours |= 8;

        var isWalled = new Func<int, bool>(w => (neighbours & w) == w);
        if (neighbours == 0x0F) // 4 walls
            offset = 64 * 5;
        else if (isWalled(1) && isWalled(2) && isWalled(4)) // 3 walls
            offset = 64 * 4;
        else if (isWalled(1) && isWalled(2) && isWalled(8))
        {
            offset = 64 * 4;
            tile.transform.Rotate(Vector3.forward, 90);
        }
        else if (isWalled(1) && isWalled(4) && isWalled(8))
        {
            offset = 64 * 4;
            tile.transform.Rotate(Vector3.forward, 180);
        }
        else if (isWalled(4) && isWalled(2) && isWalled(8))
        {
            offset = 64 * 4;
            tile.transform.Rotate(Vector3.forward, 270);
        }
        else if (isWalled(1) && isWalled(4)) // 2 walls
            offset = 64 * 2;
        else if (isWalled(2) && isWalled(8))
        {
            offset = 64 * 2;
            tile.transform.Rotate(Vector3.forward, 90);
        }
        else if (isWalled(1) && isWalled(2))
            offset = 64 * 3;
        else if (isWalled(2) && isWalled(4))
        {
            offset = 64 * 3;
            tile.transform.Rotate(Vector3.forward, 270);
        }
        else if (isWalled(4) && isWalled(8))
        {
            offset = 64 * 3;
            tile.transform.Rotate(Vector3.forward, 180);
        }
        else if (isWalled(8) && isWalled(1))
        {
            offset = 64 * 3;
            tile.transform.Rotate(Vector3.forward, 90);
        }
        else if (isWalled(1)) // 1 wall
            offset = 64;
        else if (isWalled(8))
        {
            offset = 64;
            tile.transform.Rotate(Vector3.forward, 90);
        }
        else if (isWalled(4))
        {
            offset = 64;
            tile.transform.Rotate(Vector3.forward, 180);
        }
        else if (isWalled(2))
        {
            offset = 64;
            tile.transform.Rotate(Vector3.forward, 270);
        }

        var spriteRenderer = tile.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            return;
        
        spriteRenderer.sprite = Sprite.Create(spriteRenderer.sprite.texture, new Rect(offset, 0, 64, 64), new Vector2(0.5f, 0.5f), 64 / TileSize.x);
    }

    private GameObject CreateTile(int x, int y, TileType[,] tiles)
    {
        var position = new Vector3(TileSize.x * x, TileSize.y * y);
        var type = tiles[x, y];

        var tile = Instantiate(GetTilePrefab(type), position, Quaternion.identity) as GameObject;
        
        if (type == TileType.Wall)
            SetWallTile(x, y, tiles, tile);

        if (tile == null)
            return null;

        tile.transform.parent = transform;
        DontSaveObject(tile);
        tile.name = string.Format("tile_{0}x{1}", x, y);

        return tile;
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

    public Vector2 RandomPosition()
    {
        return new Vector2((Random.value * TileWidth - 0.5f) * TileSize.x, (Random.value * TileHeight - 0.5f) * TileSize.y);
    }

    public bool SampleEmptyPosition(Vector2 AABB, out Vector2 position, int iterations = 5)
    {
        var mask = (1 << LayerMask.NameToLayer( "Tiles" )) | (1 << LayerMask.NameToLayer( "Default" ));
        do
        {
            position = RandomPosition();
            iterations--;
        } while (iterations > 0 &&
                 Physics2D.OverlapArea(position - AABB/2,
                     position + AABB/2, mask) != null);
        return iterations > 0;
    }
}
