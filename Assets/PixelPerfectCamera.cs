using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectCamera : MonoBehaviour
{
    public int Scale = 1;
    public int TileSize = 64;

    void Start()
    {
        UpdateCamera();
    }
#if UNITY_EDITOR
    void Update()
    {
        UpdateCamera();
    }
#endif
    void UpdateCamera()
    {
        camera.orthographicSize = Screen.height / (2f * TileSize * Scale);
    }
}
