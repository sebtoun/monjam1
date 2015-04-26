using UnityEngine;
using System.Collections;

public abstract class LevelGenerator : MonoBehaviour 
{
    public abstract void GenerateLevel(TiledWorld.TileType[,] tiles);
}
