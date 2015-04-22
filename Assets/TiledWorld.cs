using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

    public int Tries = 1000;
    public int RoomMinSize = 5;
    public int RoomMaxSize = 15;
    public bool SquashTheRooms = true;

    public Vector3 TileSize = Vector2.one;

    public enum TileType
    {
        Wall = 0,
        Floor,
        Void
    }

    private Dictionary<TileType, GameObject> _tileFromType;

    // ReSharper disable once UnusedMember.Local
    void Awake()
    {
        _tileFromType = new Dictionary<TileType, GameObject>()
        {
            {TileType.Void, VoidPrefab},
            {TileType.Floor, FloorPrefab},
            {TileType.Wall, WallPrefab},
        };

        if (!_generated)
            GenerateFloorPlan();
    }

    private class Room
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public bool Collide(Room other)
        {
            return !(x > other.x + other.w || x + w < other.x || y > other.y + other.h || y + h < other.y);
        }
    }

    private class Point
    {
        public int x;
        public int y;
    }

    private void GenerateFloorPlan()
    {
        var tiles = new TileType[TileWidth,TileHeight];
        tiles.Initialize();

        var rooms = new List<Room>();

        for (var i = Tries; i > 0; --i)
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

            room.w = Math.Min(room.x + room.w, TileWidth) - room.x - 1;
            room.h = Math.Min(room.y + room.h, TileHeight) - room.y - 1;
            
            rooms.Insert(0, room);
        }

        if (SquashTheRooms)
            SquashRooms(rooms);

        GenerateCorridors(tiles, rooms);

        foreach (var room in rooms)
            for (var x = room.x; x < room.x + room.w; ++x)
                for (var y = room.y; y < room.y + room.h; ++y)
                    tiles[x, y] = TileType.Floor;

        GenerateTiles(tiles);
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
                var pointA = new Point
                {
                    x = Random.Range(room.x, room.x + room.w),
                    y = Random.Range(room.y, room.y + room.h)
                };
                var pointB = new Point
                {
                    x = Random.Range(lastRoom.x, lastRoom.x + lastRoom.w),
                    y = Random.Range(lastRoom.y, lastRoom.y + lastRoom.h)
                };

                while (pointB.x != pointA.x && pointB.y != pointA.y)
                {
                    if (pointB.x != pointA.x)
                    {
                        if (pointB.x > pointA.x) --pointB.x;
                        else ++pointB.x;
                    }
                    else if (pointB.y != pointA.y)
                    {
                        if (pointB.y > pointA.y) --pointB.y;
                        else ++pointB.y;
                    }

                    tiles[pointB.x, pointB.y] = TileType.Floor;
                }
            }

            lastRoom = room;
        }   
    }

    private void GenerateTiles(TileType[,] tiles)
    {
        for (var x = 0; x < TileHeight; ++x)
            for (var y = 0; y < TileWidth; ++y)
                CreateTile(x, y, tiles[x, y]);

        _generated = true;
    }

    private void CreateTile(int x, int y, TileType type)
    {
        if (isBorder(x, y))
            type = TileType.Wall;

        var position = new Vector3(TileSize.x * x, TileSize.y * y);
        var tile = Instantiate(_tileFromType[type], position, Quaternion.identity) as GameObject;

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

    private bool isBorder(int x, int y)
    {
        return x == 0 || x == TileWidth - 1 || y == 0 || y == TileHeight - 1;
    }

    public void Reset()
    {
        var children = (from Transform child
                        in transform
                        select child.gameObject).ToList();
        children.ForEach(DestroyImmediate);
        GenerateFloorPlan();
    }
}
