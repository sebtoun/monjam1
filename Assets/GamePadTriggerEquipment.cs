using UnityEngine;
using XInputDotNetPure;

public class GamePadTriggerEquipment : MonoBehaviour
{
    public PlayerIndex index = PlayerIndex.One;

    public Hand Left;
    public Hand Right;

    private GamePadState _lastState;

    void Update ()
    {
        var currentState = GamePad.GetState( PlayerIndex.One );

        var leftTriggerDown = currentState.Triggers.Left > 0.5f;
        if ((_lastState.Triggers.Left > 0.5f) != leftTriggerDown )
        {
            Left.BroadcastMessage( leftTriggerDown ? "StartUsing" : "StopUsing", gameObject, SendMessageOptions.DontRequireReceiver );
        }

        var rightTriggerDown = currentState.Triggers.Right > 0.5f;
        if ((_lastState.Triggers.Right > 0.5f) != rightTriggerDown )
        {
            Right.BroadcastMessage( rightTriggerDown ? "StartUsing" : "StopUsing", gameObject, SendMessageOptions.DontRequireReceiver );
        }

        _lastState = currentState;
	}
}
