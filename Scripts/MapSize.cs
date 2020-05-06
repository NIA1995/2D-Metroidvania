using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSize : MonoBehaviour
{
    public Vector2 GizmosSize;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, GizmosSize);
    }
}
