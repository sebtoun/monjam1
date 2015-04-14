using UnityEngine;
using System.Collections;

public class RotateTowardsTarget : MonoBehaviour
{
    private Transform _target;
    private float _angularVelocity;
    private float _currentOrientation;
    public float Inertia = 0.2f;

    private Rigidbody2D _body;

    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Target").transform;
        _body = rigidbody2D;
    }

    void Update()
    {

        var targetOrientation = _target.position - transform.position;
        var targetAngle = Mathf.Atan2(targetOrientation.y, targetOrientation.x) * Mathf.Rad2Deg + 90;
        _currentOrientation = Mathf.SmoothDampAngle(_currentOrientation, targetAngle, ref _angularVelocity, Inertia);
    }

    void FixedUpdate()
    {
        _body.MoveRotation(_currentOrientation);
    }
}
