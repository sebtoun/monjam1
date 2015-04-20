using UnityEngine;

[ExecutionOrder("Intent")]
[RequireComponent(typeof(MobileEntity))]
public class PlayerControlled : MonoBehaviour
{
    private MobileEntity _mobile;

    void Start()
    {
        _mobile = GetComponent<MobileEntity>();
    }

    void Update()
    {
        var targetVelocity = (Input.GetAxis( "Horizontal" ) * Vector3.right + Input.GetAxis( "Vertical" ) * Vector3.up);
        if (targetVelocity.sqrMagnitude > 1)
            targetVelocity.Normalize();

        _mobile.TargetVelocity = targetVelocity;
    }
}
