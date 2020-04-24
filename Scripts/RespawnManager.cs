using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform RespawnPosition;
    public Vector2 RespawnSize;

    /* Monster Prefab */
    public GameObject TargetMonster;

    /* Monster Count */
    public List<GameObject> MonsterList = new List<GameObject>(5);

    void Start()
    {
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        if(MonsterList.Count < 5)
        {
            MosnterSpawn();
        }

        yield return new WaitForSeconds(10.0f);

        StartCoroutine(StartSpawn());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position, RespawnSize);
    }

    private void MosnterSpawn()
    {
        GameObject NewMonster = Instantiate(TargetMonster, new Vector3(Random.Range(-(RespawnSize.x / 2), (RespawnSize.x / 2)), RespawnPosition.position.y, 0), Quaternion.identity) as GameObject;

        NewMonster.GetComponent<EnemyController>().RespawnManagerInstance = this;

        NewMonster.gameObject.transform.parent = gameObject.transform;

        MonsterList.Add(NewMonster);
    }
}
