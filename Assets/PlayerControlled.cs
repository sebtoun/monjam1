using System.CodeDom;
using UnityEngine;
using System.Collections;

public class PlayerControlled : MonoBehaviour
{
    public float Speed = 10;
    public float Inertia = 0.2f;

    private Vector3 _currentAcceleration;
    private Vector3 _currentVelocity;

    private Vector3 _velocity;

    private Rigidbody2D _body;

    public Vector3 Velocity
    {
        get { return _velocity; }
    }

    void Awake()
    {
        _body = GetComponentInChildren<Rigidbody2D>();
    }


    void Update()
    {
        _velocity = (Input.GetAxis( "Horizontal" ) * Vector3.right + Input.GetAxis( "Vertical" ) * Vector3.up);
        if (_velocity.sqrMagnitude > 1)
            _velocity.Normalize();

        _currentVelocity = Vector3.SmoothDamp( _currentVelocity, _velocity * Speed, ref _currentAcceleration, Inertia );
    }

    void FixedUpdate()
    {
        _body.velocity = _currentVelocity;
    }
    
}
