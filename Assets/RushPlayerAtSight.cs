using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RushTarget))]
[ExecutionOrder("Controller")]
public class RushPlayerAtSight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject + " entering zone " + this);
        BroadcastMessage("SetTarget", other.transform, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerExit2D( Collider2D other )
    {
        // Debug.Log( other.gameObject + " exiting zone " + this );
        BroadcastMessage( "UnsetTarget", other.transform, SendMessageOptions.DontRequireReceiver );
    }
}
