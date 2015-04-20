using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class TiledWorld : MonoBehaviour
{
    public int TileWidth = 32;
    public int TileHeight = 32;

    private bool _generated;

    public GameObject FloorPrefab;
    public GameObject WallPrefab;
    public GameObject VoidPrefab;

    public int RoomMinSize = 5;
    public int RoomMaxSize = 10;

    public Vector3 TileSize = Vector2.one;

    public enum TileType
    {
        Wall = 0,
        Floor,
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
            GenerateTiles();
    }

    private class Room
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public bool Collide(Room other)
        {
            return x > other.x + other.w || x + w < other.x || y > other.y + other.h || y + h < other.y;
        }

        public float SquaredDistance(Room other)
        {
            return (float)(Math.Pow((x + w) / 2 - (other.x + other.w) / 2, 2) + Math.Pow((y + h) / 2 - (other.y + other.h) / 2, 2));
        }
    }

    private void GenerateFloorPlan()
    {
        var tiles = new TileType[TileWidth,TileHeight];
        tiles.Initialize();

        var roomCount = Random.Range(10, 20);
        var rooms = new List<Room>();

        while (rooms.Count < roomCount)
        {
            var room = new Room
            {
                x = Random.Range(1, TileWidth),
                y = Random.Range(1, TileHeight),
                w = Random.Range(RoomMinSize, RoomMaxSize),
                h = Random.Range(RoomMinSize, RoomMaxSize)
            };

            if (rooms.Any(r => r.Collide(room)))
                continue;

            room.w -= 1;
            room.h -= 1;
            
            rooms.Insert(0, room);
        }

        SquashRooms(rooms);
        GenerateCorridors(tiles, rooms);
    }

    private static void SquashRooms(List<Room> rooms)
    {
        rooms.Sort(delegate(Room l, Room r)
        {
            if (l.x > r.x)
                return 1;
            if (l.x == r.x && l.y > r.y)
                return 1;
            if (l.x == r.x && l.y == r.y)
                return 0;
            return -1;
        });

        var lastRoom = new Room {x = 1, y = 1, w = 0, h = 0};
        foreach (var room in rooms)
        {
            room.x = Math.Min(Math.Min(room.x, 1), lastRoom.x + lastRoom.w + 1);
            room.y = Math.Min(Math.Min(room.y, 1), lastRoom.y + lastRoom.w + 1);
            lastRoom = room;
        }
    }

    private void GenerateCorridors(TileType[,] tiles, List<Room> rooms)
    {
        Room lastRoom = null;
        foreach (var room in rooms)
        {
            if (lastRoom != null)
            {
                
            }

            lastRoom = room;
        }   
    }

    private void GenerateTiles()
    {
        for (var y = 0; y < TileHeight; ++y)
        {
            for (var x = 0; x < TileWidth; ++x)
            {
                Debug.Log( _tileFromType[GetTileType( x, y )] );
                CreateTile(x, y);
            }
        }

        _generated = true;
    }

    private void CreateTile(int x, int y)
    {
        var position = new Vector3(TileSize.x * x, TileSize.y * y);
        var tile = Instantiate(_tileFromType[GetTileType(x, y)], position, Quaternion.identity) as GameObject;

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
            child.gameObject.hideFlags = HideFlags.DontSave;
            DontSaveObject(child.gameObject);
        }
    }

    private TileType GetTileType(int x, int y)
    {
        if (x == 0 || x == TileWidth - 1 || y == 0 || y == TileHeight - 1)
            return TileType.Wall;
        return TileType.Floor;
    }
}
