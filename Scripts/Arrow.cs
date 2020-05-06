using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Vector3 TargetPosition;

    SpriteRenderer Renderer;

    int Damage = 1;

    float Speed = 10.0f;

    public void Set(Vector3 StartPos, int Dir, int Power)
    {
        TargetPosition = StartPos;

        if (Dir > 0)
        {
            TargetPosition = new Vector3(StartPos.x + 10, StartPos.y, StartPos.z);
            Renderer.flipX = true;
        }
        else
        {
            TargetPosition = new Vector3(StartPos.x - 10, StartPos.y, StartPos.z);
        }

        transform.position = StartPos;
    }

    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);

        if (transform.position == TargetPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.gameObject.tag == "Enemy")
        {
            Collision.gameObject.GetComponent<EnemyController>().GetDamage(Damage, transform.position, false);
            Destroy(gameObject);
        }
    }
}
