using UnityEngine;
using System.Collections;

[ExecutionOrder("Camera")]
public class FollowPlayerAndTarget : MonoBehaviour
{
    private Transform _player;
    private Transform _target;

    private float _cameraDistance;
    public float TrackingSpeed = 1;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _target = GameObject.FindGameObjectWithTag( "Target" ).transform;

        var cam = Camera.main;
        var diag = cam.ViewportToWorldPoint( Vector2.one ) - cam.ViewportToWorldPoint( Vector2.zero );
        
        _cameraDistance = 0.1f*Mathf.Min(diag.x, diag.y);
    }

    void LateUpdate()
    {
        var playerPos = _player.position;
        var targetPos = _target.position;

        var cameraTargetPosition = playerPos + (targetPos - playerPos).normalized * _cameraDistance;
        cameraTargetPosition.z = transform.position.z;

        this.transform.Translate((cameraTargetPosition - transform.position) * Time.deltaTime * TrackingSpeed);
    }
}
