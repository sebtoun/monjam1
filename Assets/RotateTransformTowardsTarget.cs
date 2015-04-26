using UnityEngine;
using System.Collections;

public class RotateTransformTowardsTarget : MonoBehaviour 
{
    public Transform Target;
    private float _angularVelocity;
    private float _currentOrientation;
    public float Inertia = 0.2f;
    private Transform _tr;

    void Start()
    {
        _tr = transform;
    }

    void LateUpdate()
    {
        if (Target != null)
        {
            var targetOrientation = Target.position - transform.position;
            var targetAngle = Mathf.Atan2(targetOrientation.y, targetOrientation.x)*Mathf.Rad2Deg - 90;
            _currentOrientation = Mathf.SmoothDampAngle(_currentOrientation, targetAngle, ref _angularVelocity, Inertia);
            _tr.rotation = Quaternion.AngleAxis(_currentOrientation, Vector3.forward);
        }
    }

    void SetTarget( Transform target )
    {
        Target = target;
    }

    void UnsetTarget( Transform target )
    {
        if (Target == target)
        {
            Target = null;
        }
    }
}
