using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Animator ) )]
public class ChangeStateOnHit : MonoBehaviour
{
    public float StandUpDelay = 1.5f;
    private Animator _state;

    void Awake()
    {
        _state = GetComponent<Animator>();
    }

    void Hit( Vector3 strikeVector )
    {
        _state.SetTrigger( "Hit" );
        rigidbody2D.AddForce( strikeVector, ForceMode2D.Impulse );
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
        yield return new WaitForSeconds(StandUpDelay);
        _state.SetTrigger( "GetUp" );
    }
}
