using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public delegate void InPortal();
    public static event InPortal PortalEvent;

    public MapSize NextMapSize;

    public Transform TargetPosition;

    public bool IsLeft;
    public int NextMapIndex;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if(Collision.CompareTag("Player"))
        {
            if(IsLeft)
            {
                PlayerController.Instance.transform.position = new Vector3(TargetPosition.position.x - 1, TargetPosition.position.y, TargetPosition.position.z);
            }
            else
            {
                PlayerController.Instance.transform.position = new Vector3(TargetPosition.position.x + 1, TargetPosition.position.y, TargetPosition.position.z);
            }

            PortalEvent();

            NewCamera.Instance.Center = NextMapSize.transform.position;
            NewCamera.Instance.Size = NextMapSize.GizmosSize;

            MapSystem.Instance.ChangeNowMap(NextMapIndex);
        }
    }
}
