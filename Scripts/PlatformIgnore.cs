using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformIgnore : MonoBehaviour
{
    public Collider2D TargetCollider;

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if(Collision.CompareTag("Player") && Input.GetAxisRaw("Vertical") < 0)
        {
            Physics2D.IgnoreCollision(Collision.GetComponent<Collider2D>(), TargetCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Player"))
        {

            Physics2D.IgnoreCollision(Collision.GetComponent<Collider2D>(), TargetCollider, false);
        }
    }
}
