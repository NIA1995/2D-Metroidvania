using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Vector2 RespawnBoxSize;
    public Transform RespawnPosition;

    public GameObject TargetMonster;

    private GameObject RespawnedMonster;

    private void Start()
    {
        MonsterSpawn();

        Portal.PortalEvent += MonsterSpawn;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(gameObject.transform.position, RespawnBoxSize);
    }

    private void MonsterSpawn()
    {
        if(RespawnedMonster == null)
        {
            RespawnedMonster = Instantiate(TargetMonster, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0), Quaternion.identity) as GameObject;
        }

        RespawnedMonster.gameObject.transform.parent = gameObject.transform;
        RespawnedMonster.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, 0);
    }

}
