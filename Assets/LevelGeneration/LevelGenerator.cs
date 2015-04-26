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
}
