using UnityEngine;
using System.Collections;

[ExecutionOrder("Intent")]
public class RotateTowardsTarget : MonoBehaviour
{
    private Transform _target;
    private MobileEntity _mobile;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag( "Target" ).transform;
        _mobile = GetComponent<MobileEntity>();
    }

    void Update()
    {

        var targetOrientation = _target.position - transform.position;
        _mobile.TargetAngle = Mathf.Atan2(targetOrientation.y, targetOrientation.x) * Mathf.Rad2Deg - 90;
    }
}
