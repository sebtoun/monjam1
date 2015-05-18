using UnityEngine;
using System.Collections;

public class StrikeOnUse : MonoBehaviour
{
    public LayerMask HitMask;
    public float RaiseDuration;
    public float RecoveryDuration;

    public Vector2 RelativeDirection;
    public float StrikeDistance;
    public float StrikePower;
    public float StrikeRadius;
    
    private bool _isStriking;

    void StartUsing(GameObject entity)
    {
        if (_isStriking)
        {
            return;
        }
        _isStriking = true;
        StartCoroutine( Strike(entity) );
    }

    private IEnumerator Strike(GameObject entity)
    {
        _lastEntity = entity;
        yield return new WaitForSeconds(RaiseDuration);

        var strikeDirection = transform.TransformDirection(RelativeDirection);
        
        //RaycastHit2D hit;
        //if ((hit = Physics2D.Raycast(transform.position, strikeDirection, StrikeDistance, HitMask)).collider != null)
        //{
        //    hit.rigidbody.AddForce(strikeDirection * StrikePower, ForceMode2D.Impulse);
        //}
        
        var strikePos = entity.transform.position + StrikeDistance * strikeDirection;
        foreach (var col in Physics2D.OverlapCircleAll(strikePos, StrikeRadius, HitMask))
        {
            col.GetComponentInParent<Rigidbody2D>().SendMessage( "Hit", strikeDirection * StrikePower );            
        }
         
        yield return new WaitForSeconds(RecoveryDuration);
        _isStriking = false;
    }

    private GameObject _lastEntity;
    void OnDrawGizmos()
    {
        if (_lastEntity != null)
        {
            var strikePos = _lastEntity.transform.position + StrikeDistance * transform.TransformDirection( RelativeDirection );

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere( strikePos, StrikeRadius );
        }
    }
}
