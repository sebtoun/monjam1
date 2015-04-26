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
    public virtual void GenerateLevel(TiledWorld.TileType[,] tiles)
    {
        var worldGen = GenerateLevelStepByStep(tiles);
        while (worldGen.MoveNext())
            ; // do nothing
    }

    /// <summary>
    /// Generate level tiles steps by steps. Default implementation create the full level in one step.
    /// </summary>
    /// <param name="tiles">The tile matrix representing the level.</param>
    /// <returns>An enumerator to be started as a Coroutine.</returns>
    public virtual IEnumerator GenerateLevelStepByStep(TiledWorld.TileType[,] tiles)
    {
        GenerateLevel(tiles);
        yield break;
    }

    protected void FillWithType( TiledWorld.TileType[,] tiles, TiledWorld.TileType type = TiledWorld.TileType.Wall )
    {
        var width = tiles.GetLength( 0 );
        var height = tiles.GetLength( 1 );
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                tiles.SetValue(type, i, j);
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
