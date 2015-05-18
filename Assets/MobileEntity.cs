using UnityEngine;
using System.Collections;

[ExecutionOrder("Movement")]
public class MobileEntity : MonoBehaviour 
{
    public float BackwardSpeed = 6;
    public float ForwardSpeed = 10;
    public float InertiaTranslation = 0.1f;
    public float InertiaRotation = 0.1f;
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

    private Animator _state;
    void Start()
    {
        _state = GetComponent<Animator>();
        _body = GetComponentInChildren<Rigidbody2D>();
        BackwardSpeed = BackwardSpeed * (1 + Random.value * ParametersRandomization);
        ForwardSpeed = ForwardSpeed * (1 + Random.value * ParametersRandomization);
        InertiaTranslation = InertiaTranslation * (1 + Random.value * ParametersRandomization);
        InertiaRotation = InertiaRotation * (1 + Random.value * ParametersRandomization);
        VelocityGain = VelocityGain * (1 + Random.value * ParametersRandomization);
    }

    void Update()
    {
        var actualSpeed = Mathf.Lerp( BackwardSpeed, ForwardSpeed, 0.5f * ( Vector2.Dot( transform.up, TargetVelocity ) + 1 ) );

        _currentVelocity = Vector2.SmoothDamp( _currentVelocity, TargetVelocity * actualSpeed, ref _currentAcceleration, InertiaTranslation );
        _currentAngle = Mathf.SmoothDampAngle( _currentAngle, TargetAngle, ref _angularVelocity, InertiaRotation );
    }

    void FixedUpdate()
    {
        if (_state != null && _state.GetCurrentAnimatorStateInfo(0).IsName("Inert"))
        {
            return;
        }
        _body.MovePosition( _body.position + _currentVelocity * Time.fixedDeltaTime );
        //_body.AddForce( (_currentVelocity - _body.velocity) * VelocityGain, ForceMode2D.Force );
        _body.MoveRotation( _currentAngle );
    }
}
