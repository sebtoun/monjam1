using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefaultGenerator : LevelGenerator 
{
    public int Tries = 1000;
    public int RoomMinSize = 5;
    public int RoomMaxSize = 15;
    public bool SquashTheRooms = true;

    public int TileWidth = 32;
    public int TileHeight = 32;

    private class Point
    {
        public int x;
        public int y;
    }

    private class Room
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public bool Collide( Room other )
        {
            return !(x > other.x + other.w || x + w < other.x || y > other.y + other.h || y + h < other.y);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Room;
            return other != null && x == other.x && y == other.y && w == other.w && h == other.h;
        }

        public float SquaredDistance(Room other)
        {
            var thisCenter = new Vector2(x + w / 2, y + h / 2);
            var otherCenter = new Vector2(other.x + other.w / 2, other.y + other.h / 2);
            return (otherCenter - thisCenter).sqrMagnitude;
        }

        public Room FindClosestRoom(Room[] rooms, int i)
        {
            Room closest = null;
            var distance = float.MaxValue;
            for (; i < rooms.Length; ++i)
            {
                var room = rooms[i];
                var dist = SquaredDistance(room);
                if (Equals(room, this) || !(dist < distance)) continue;
                closest = room;
                distance = dist;
            }
            return closest;
        }
    }

    public override IEnumerator GenerateLevelStepByStep()
    {
        var width = TileWidth;
        var height = TileHeight;

        var tiles = new TiledWorld.TileType[width, height];
        InitializeWithType(tiles);

        yield return tiles;

        var rooms = new List<Room>();

        for (var i = Tries; i > 0; --i)
        {
            var room = new Room
            {
                x = Random.Range( 1, width - RoomMinSize ),
                y = Random.Range( 1, height - RoomMinSize ),
                w = Random.Range(RoomMinSize, RoomMaxSize),
                h = Random.Range(RoomMinSize, RoomMaxSize)
            };

            if (rooms.Any(r => r.Collide(room)))
                continue;

            room.w = Math.Min(room.x + room.w, width) - room.x - 1;
            room.h = Math.Min(room.y + room.h, height) - room.y - 1;

            rooms.Insert(0, room);
        }

        foreach (var room in rooms)
        {
            FillAreaWithType(tiles, room.x, room.y, room.w, room.h, TiledWorld.TileType.Floor);
        }
        yield return tiles;

        GenerateCorridors( tiles, rooms );

        yield return tiles;

        CloseBorders(tiles);
        
        yield return tiles;
    }

    private void GenerateCorridors( TiledWorld.TileType[,] tiles, IEnumerable<Room> rooms )
    {
        var arooms = rooms.ToArray();
        for (var i = 0; i < arooms.Length; ++i)
        {
            var roomA = arooms[i];
            var roomB = roomA.FindClosestRoom(arooms, i + 1);
            if (roomB == null)
                continue;

            var pointA = new Point
            {
                x = Random.Range( roomA.x, roomA.x + roomA.w ),
                y = Random.Range( roomA.y, roomA.y + roomA.h )
            };
            var pointB = new Point
            {
                x = Random.Range( roomB.x, roomB.x + roomB.w ),
                y = Random.Range( roomB.y, roomB.y + roomB.h )
            };

            while (!(pointB.x == pointA.x && pointB.y == pointA.y))
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

                tiles[pointB.x, pointB.y] = TiledWorld.TileType.Floor;
            }
        }
    }

    
}
