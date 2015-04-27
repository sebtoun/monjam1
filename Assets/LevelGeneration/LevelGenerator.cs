using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Astract class for level generation. You should at least override one of the two generation methods since default implementations use each other.
/// </summary>
public abstract class LevelGenerator : MonoBehaviour 
{
    /// <summary>
    /// Generate the whole Level at once.
    /// </summary>
    /// <param name="tiles">The tile matrix representing the level.</param>
    public virtual TiledWorld.TileType[,] GenerateLevel()
    {
        var worldGen = GenerateLevelStepByStep();
        while (worldGen.MoveNext())
            ; // do nothing
        return worldGen.Current as TiledWorld.TileType[,];
    }

    /// <summary>
    /// Generate level tiles steps by steps. Default implementation create the full level in one step.
    /// </summary>
    /// <param name="tiles">The tile matrix representing the level.</param>
    /// <returns>An enumerator to be started as a Coroutine.</returns>
    public virtual IEnumerator GenerateLevelStepByStep()
    {
        yield return GenerateLevel();
    }

    /// <summary>
    /// Fill an area of tiles with the given type
    /// </summary>
    /// <param name="tiles">the tiles matrix</param>
    /// <param name="x">left most tile</param>
    /// <param name="y">bottom most tile</param>
    /// <param name="w">area width in tiles</param>
    /// <param name="h">area height in tiles</param>
    /// <param name="type"></param>
    protected void FillAreaWithType( TiledWorld.TileType[,] tiles, int x, int y, int w, int h, TiledWorld.TileType type)
    {
        var width = tiles.GetLength( 0 );
        var height = tiles.GetLength( 1 );
        for (int i = x; i < Math.Min(x + w, width); i++)
            for (int j = y; j < Math.Min(y + h, height); j++)
                tiles[i, j] = type;
    }

    protected void InitializeWithType(TiledWorld.TileType[,] tiles, TiledWorld.TileType type = TiledWorld.TileType.Wall)
    {
        FillAreaWithType(tiles, 0, 0, tiles.GetLength(0), tiles.GetLength(1), type);
    }

    protected void CloseBorders(TiledWorld.TileType[,] tiles, TiledWorld.TileType type = TiledWorld.TileType.Wall)
    {
        var width = tiles.GetLength(0);
        var height = tiles.GetLength(1);

        for (int i = 0; i < width; ++i)
            tiles[i, 0] = type;
        for (int i = 0; i < width; ++i)
            tiles[i, height - 1] = type;

        for (int i = 0; i < height; ++i)
            tiles[0, i] = type;
        for (int i = 0; i < height; ++i)
            tiles[width - 1, i] = type;
    }
}
