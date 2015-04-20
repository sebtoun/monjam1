using UnityEngine;
using System.Collections;

[ExecutionOrder("Movement")]
public class MobileEntity : MonoBehaviour 
{
    public float Speed = 10;
    public float Inertia = 0.1f;
    public float VelocityGain = 8;

    [Range(0, 1)]
    public float ParametersRandomization = 0;

    private Vector2 _currentAcceleration;
    private Vector2 _currentVelocity;

    private float _angularVelocity;
    private float _currentAngle;
    
    private Rigidbody2D _body;

    [HideInInspector]
    public Vector2 TargetVelocity;
    [HideInInspector]
    public float TargetAngle;
    
    void Start()
    {
        _body = GetComponentInChildren<Rigidbody2D>();
        Speed = Speed*(1 + Random.value*ParametersRandomization);
        Inertia = Inertia*(1 + Random.value*ParametersRandomization);
        VelocityGain = VelocityGain * (1 + Random.value * ParametersRandomization);
    }

    void Update()
    {
        _currentVelocity = Vector2.SmoothDamp( _currentVelocity, TargetVelocity * Speed, ref _currentAcceleration, Inertia );
        _currentAngle = Mathf.SmoothDampAngle( _currentAngle, TargetAngle, ref _angularVelocity, Inertia );
    }

    void FixedUpdate()
    {
        // _body.velocity = _currentVelocity;
        _body.AddForce( (_currentVelocity - _body.velocity) * VelocityGain, ForceMode2D.Force );
        _body.MoveRotation( _currentAngle );
    }
}
