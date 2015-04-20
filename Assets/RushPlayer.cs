using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MobileEntity))]
public class RushPlayer : MonoBehaviour 
{
    private MobileEntity _mobile;
    private Transform _target;

    void Start()
    {
        _mobile = GetComponent<MobileEntity>();
        _target = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }

    void Update()
    {
        var targetOrientation = _target.position - transform.position;
        _mobile.TargetAngle = Mathf.Atan2( targetOrientation.y, targetOrientation.x ) * Mathf.Rad2Deg - 90;
        _mobile.TargetVelocity = targetOrientation.normalized;
    }
}
