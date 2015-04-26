using UnityEngine;

[RequireComponent( typeof( MobileEntity ) )]
[ExecutionOrder( "Intent" )]
public class RushTarget : MonoBehaviour 
{
    private MobileEntity _mobile;
    public Transform Target;

    void Start()
    {
        _mobile = GetComponent<MobileEntity>();
    }

    void Update()
    {
        if (Target)
        {
            var targetOrientation = Target.position - transform.position;
            _mobile.TargetAngle = Mathf.Atan2(targetOrientation.y, targetOrientation.x)*Mathf.Rad2Deg - 90;
            _mobile.TargetVelocity = targetOrientation.normalized;
        }
        else
        {
            _mobile.TargetVelocity = Vector2.zero;
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
