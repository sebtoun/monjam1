using UnityEngine;
using System.Collections;

public class ImageLoader : LevelGenerator
{
    public Texture2D LevelImage;

    public TiledWorld.TileType getTileAt(Color32[] pixels, int x, int y)
    {
        return getTileForColor(pixels[y*LevelImage.width + x]);
    }

    public TiledWorld.TileType getTileForColor(Color32 color)
    {
        if (color == Color.white)
        {
            return TiledWorld.TileType.Floor;
        }
        return TiledWorld.TileType.Wall;
    }

    public override IEnumerator GenerateLevelStepByStep()
    {
        var pixels = LevelImage.GetPixels32();
        var tiles = new TiledWorld.TileType[LevelImage.width, LevelImage.height];
        
        for (var x = 0; x < LevelImage.width; ++x)
            for (var y = 0; y < LevelImage.height; ++y)
            {
                tiles[x, y] = getTileAt(pixels, x, y);
                yield return tiles;
            }

        CloseBorders(tiles);
        yield return tiles;
    }
}
