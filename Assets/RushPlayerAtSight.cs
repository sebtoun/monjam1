﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RushTarget))]
public class RushPlayerAtSight : MonoBehaviour
{
    private RushTarget _rush;

    void Start()
    {
        _rush = GetComponent<RushTarget>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject + " entering zone " + this);
        _rush.Target = other.transform;
    }

    void OnTriggerExit2D( Collider2D other )
    {
        // Debug.Log( other.gameObject + " exiting zone " + this );
        if (other.transform == _rush.Target)
            _rush.Target = null;
    }
}
