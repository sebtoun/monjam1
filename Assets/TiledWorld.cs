using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TiledWorld : MonoBehaviour
{
    public int TileWidth = 32;
    public int TileHeight = 32;
    public int radius = 12;

    private bool _generated;

    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject VoidPrefab;

    public Vector3 TileSize = Vector2.one;

    public enum TileType
    {
        Floor,
        Wall,
        Void
    }

    private Dictionary<TileType, GameObject> _tileFromType;

    void Awake()
    {
        _tileFromType = new Dictionary<TileType, GameObject>()
        {
            {TileType.Void, VoidPrefab},
            {TileType.Floor, FloorPrefab},
            {TileType.Wall, WallPrefab},
        };
        if (!_generated)
            this.GenerateTiles();
    }

    public void GenerateTiles()
    {
        var root = transform;
        var y = (-TileHeight / 2 + 0.5f) * TileSize.y;
        
        for (var r = -TileHeight/2; r < TileHeight/2; ++r)
        {
            var x = (-TileWidth / 2 + 0.5f) * TileSize.x;    
            
            for (var c = -TileWidth/2; c < TileWidth/2; ++c)
            {
                var tile = Instantiate( _tileFromType[GetTileType( c, r )], new Vector3( x, y ), Quaternion.identity ) as GameObject;
                tile.transform.parent = root;
                DontSaveObject(tile);
                tile.name = string.Format( "tile_{0}x{1}", r, c );
            
                x += TileSize.x;
            }
            
            y += TileSize.y;
        }

        _generated = true;
    }

    private static void DontSaveObject(GameObject obj)
    {
        obj.hideFlags = HideFlags.DontSave;
        foreach (Transform child in obj.transform)
        {
            child.gameObject.hideFlags = HideFlags.DontSave;
            DontSaveObject(child.gameObject);
        }
    }

    private TileType GetTileType(int x, int y)
    {
        if (x * x + y * y < radius * radius)
            return TileType.Floor;
        else if (x * x + y * y < (radius + 2) * (radius + 2))
            return TileType.Wall;
        else 
            return TileType.Void;
    }
}
