using UnityEngine;
using System.Collections;

[ExecutionOrder("Intent")]
public class RotateTowardsTarget : MonoBehaviour
{
    public Transform Target;
    private MobileEntity _mobile;

    void Start()
    {
        if (Target == null)
            Target = GameObject.FindGameObjectWithTag( "Target" ).transform;
        _mobile = GetComponent<MobileEntity>();
    }

    void Update()
    {

        var targetOrientation = Target.position - transform.position;
        _mobile.TargetAngle = Mathf.Atan2(targetOrientation.y, targetOrientation.x) * Mathf.Rad2Deg - 90;
    }

    void SetTarget(Transform target)
    {
        Target = target;
    }

    void UnsetTarget(Transform target)
    {
        if (Target == target)
        {
            Target = null;
        }
    }
}
