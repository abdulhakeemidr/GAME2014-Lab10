using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Player Detection")] 
    public LOS enemyLOS;

    [Header("Movement")] 
    public float runForce;
    public Transform lookAheadPoint;
    public Transform lookInFrontPoint;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public bool isGroundAhead;

    [Header("Animation")] 
    public Animator animatorController;

    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemyLOS = GetComponent<LOS>();
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAhead();
        LookInFront();

        if (!HasLOS())
        {
            animatorController.enabled = true;
            animatorController.Play("Run");
            MoveEnemy();
        }
        else
        {
            animatorController.enabled = false;
        }
        
    }

    private bool HasLOS()
    {
        if (enemyLOS.colliderList.Count > 0)
        {
            // Case 1 enemy polygonCollider2D collides with player and player is at the top of the list
            if ((enemyLOS.collidesWith.gameObject.CompareTag("Player")) &&
                (enemyLOS.colliderList[0].gameObject.CompareTag("Player")))
            {
                return true;
            }
            // Case 2 player is in the Collider List and we can draw ray to the player
            else
            {
                foreach (var collider in enemyLOS.colliderList)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        var hit = Physics2D.Raycast(lookInFrontPoint.position, Vector3.Normalize(collider.transform.position - lookInFrontPoint.position), 5.0f, enemyLOS.contactFilter.layerMask);
                        Debug.DrawLine(lookInFrontPoint.position, collider.transform.position, Color.red);
                        if((hit) && (hit.collider.gameObject.CompareTag("Player")))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    private void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, lookAheadPoint.position, groundLayerMask);
        isGroundAhead = (hit) ? true : false;
    }

    private void LookInFront()
    {
        var hit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, wallLayerMask);
        if (hit)
        {
            Flip();
        }
    }

    private void MoveEnemy()
    {
        if (isGroundAhead)
        {
            rigidbody.AddForce(Vector2.left * runForce * transform.localScale.x);
            rigidbody.velocity *= 0.90f;
        }
        else
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    // EVENTS

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }


    // UTILITIES
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, lookAheadPoint.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, lookInFrontPoint.position);

        //foreach (var collider in enemyLOS.colliderList)
        //{
        //    if (collider.gameObject.CompareTag("Player"))
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawLine(lookInFrontPoint.position, collider.transform.position);
        //    }
        //}
    }
}
