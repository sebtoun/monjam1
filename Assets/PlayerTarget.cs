using UnityEngine;
using System.Collections;

[ExecutionOrder( "Controller" )]
public class PlayerTarget : MonoBehaviour
{
    public float Height = -1;

    private Transform _player;
    private Vector2 _localPos;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        Screen.lockCursor = true;
        Screen.showCursor = false;
    }

    void Update()
    {
        _localPos += Input.GetAxis( "Mouse X" ) * Vector2.right + Input.GetAxis( "Mouse Y" ) * Vector2.up;

        transform.position = _player.position + (Vector3)_localPos + Height * Vector3.forward;
    }

}
