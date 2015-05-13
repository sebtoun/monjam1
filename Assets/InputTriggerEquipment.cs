using UnityEngine;

public class InputTriggerEquipment : MonoBehaviour 
{
    public Hand Left;
    public Hand Right;

	void Update () 
    {
	    if (Input.GetButtonDown( "Left" ))
	    {
            Left.BroadcastMessage( "StartUsing", gameObject, SendMessageOptions.DontRequireReceiver );
	    }
	    else if (Input.GetButtonUp( "Left" ))
	    {
            Left.BroadcastMessage( "StopUsing", gameObject, SendMessageOptions.DontRequireReceiver );
	    }

        if (Input.GetButtonDown( "Right" ))
        {
            Right.BroadcastMessage( "StartUsing", gameObject, SendMessageOptions.DontRequireReceiver );
        }
        else if (Input.GetButtonUp( "Right" ))
        {
            Right.BroadcastMessage( "StopUsing", gameObject, SendMessageOptions.DontRequireReceiver );
        }
	}
}
