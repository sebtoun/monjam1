using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class DefaultGenerator : LevelGenerator 
{
    public int Tries = 1000;
    public int RoomMinSize = 5;
    public int RoomMaxSize = 15;
    public bool SquashTheRooms = true;


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
    }

    private class Point
    {
        public int x;
        public int y;
    }

    public override IEnumerator GenerateLevelStepByStep(TiledWorld.TileType[,] tiles)
    {
        var width = tiles.GetLength(0);
        var height = tiles.GetLength(1);

        FillWithType(tiles);

        yield return null;

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

        if (SquashTheRooms)
            SquashRooms(rooms);

        foreach (var room in rooms)
        {
            for (var x = room.x; x < room.x + room.w; ++x)
                for (var y = room.y; y < room.y + room.h; ++y)
                    tiles[x, y] = TiledWorld.TileType.Floor;

            yield return null;
        }

        GenerateCorridors( tiles, rooms );

        yield return null;

        CloseBorders(tiles);
    }

    private void SquashRooms( List<Room> rooms )
    {
        rooms.Sort( delegate( Room l, Room r )
        {
            if (l.x > r.x)
                return 1;
            if (l.x == r.x && l.y > r.y)
                return 1;
            if (l.x == r.x && l.y == r.y)
                return 0;
            return -1;
        } );

        var lastRoom = new Room { x = 1, y = 1, w = 0, h = 0 };
        foreach (var room in rooms)
        {
            room.x = Math.Min( Math.Min( room.x, 1 ), lastRoom.x + lastRoom.w + 1 );
            room.y = Math.Min( Math.Min( room.y, 1 ), lastRoom.y + lastRoom.w + 1 );
            lastRoom = room;
        }
    }

    private void GenerateCorridors( TiledWorld.TileType[,] tiles, List<Room> rooms )
    {
        Room lastRoom = null;
        foreach (var room in rooms)
        {
            if (lastRoom != null)
            {
                var pointA = new Point
                {
                    x = Random.Range( room.x, room.x + room.w ),
                    y = Random.Range( room.y, room.y + room.h )
                };
                var pointB = new Point
                {
                    x = Random.Range( lastRoom.x, lastRoom.x + lastRoom.w ),
                    y = Random.Range( lastRoom.y, lastRoom.y + lastRoom.h )
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

                    tiles[pointB.x, pointB.y] = TiledWorld.TileType.Floor;
                }
            }

            lastRoom = room;
        }
    }

    
}
