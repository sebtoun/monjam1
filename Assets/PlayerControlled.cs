using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Security.Policy;

public class PlayerControlled : MonoBehaviour
{
    public float Speed = 10;
    public float Inertia = 0.2f;

    private Vector2 _currentAcceleration;
    private Vector2 _currentVelocity;

    private Vector2 _targetVelocity;

    private Rigidbody2D _body;

    public Vector3 TargetVelocity
    {
        get { return _targetVelocity; }
    }

    void Awake()
    {
        _body = GetComponentInChildren<Rigidbody2D>();
    }


    void Update()
    {
        _targetVelocity = (Input.GetAxis( "Horizontal" ) * Vector3.right + Input.GetAxis( "Vertical" ) * Vector3.up);
        if (_targetVelocity.sqrMagnitude > 1)
            _targetVelocity.Normalize();
        
        _currentVelocity = Vector2.SmoothDamp( _currentVelocity, _targetVelocity * Speed, ref _currentAcceleration, Inertia );
    }

    public float PositionalGain = 2;
    void FixedUpdate()
    {
//        _body.velocity = _currentVelocity;
        _body.AddForce( (_currentVelocity - _body.velocity) * PositionalGain, ForceMode2D.Force );
    }
    
}
