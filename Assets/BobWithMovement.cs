using UnityEngine;
using System.Collections;

[ExecutionOrder("Camera")]
public class BobWithMovement : MonoBehaviour
{
    private float _phase;
    public float Amplitude = 15;
    public float Period = 5;

    private Vector2 _lastPosition;

    void Start()
    {
        _lastPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector2 pos = transform.position;

        _phase += ( pos - _lastPosition ).magnitude * 2 * Mathf.PI / Period;
        transform.localRotation = Quaternion.AngleAxis( Mathf.Sin( _phase ) * Amplitude, Vector3.forward );

        _lastPosition = pos;
    }

}
