using UnityEngine;
using System.Collections;

public class RotateTransformTowardsTarget : MonoBehaviour 
{
    private Transform _target;
    private float _angularVelocity;
    private float _currentOrientation;
    public float Inertia = 0.2f;
    private Transform _tr;

    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag( "Target" ).transform;
        _tr = transform;
    }

    void LateUpdate()
    {

        var targetOrientation = _target.position - transform.position;
        var targetAngle = Mathf.Atan2( targetOrientation.y, targetOrientation.x ) * Mathf.Rad2Deg - 90;
        _currentOrientation = Mathf.SmoothDampAngle( _currentOrientation, targetAngle, ref _angularVelocity, Inertia );
        _tr.rotation = Quaternion.AngleAxis(_currentOrientation, Vector3.forward);
    }
}
