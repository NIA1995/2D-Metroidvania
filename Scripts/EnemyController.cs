using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float MoveSpeed;
    bool IsLeftSide = true;
    public float HP = 100;
    bool IsHurt = false;
    bool IsKnockBack = false;

    Animator EnemyAnimator;
    Rigidbody2D RigidBody;
    SpriteRenderer Renderer;

    Color AlphaA = Color.red;
    Color AlphaB = new Color(1, 1, 1, 1);
    public float KnockBackPower;

    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(!IsKnockBack)
        {
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
            EnemyAnimator.SetBool("IsMoving", true);
        }
        else
        {
            EnemyAnimator.SetBool("IsMoving", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if(Collision.tag == "EndPoint")
        {
            if(IsLeftSide)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                IsLeftSide = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                IsLeftSide = true;
            }
        }
    }

    public void GetDamage(int Damage, Vector2 Pos, bool LastAttack)
    {
        IsHurt = true;
        HP -= Damage;

        if (HP < 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            if (LastAttack)
            {
                float x = transform.position.x - Pos.x;

                if (x < 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }

                StartCoroutine(KnockBack(x));
            }

            StartCoroutine(AlphaBlink());
            StartCoroutine(SetHurt());
;        }
    }

    IEnumerator KnockBack(float Direction)
    {
        IsKnockBack = true;

        Debug.Log(Direction);

        float KnockBackTime = 0.0f;

        while(KnockBackTime < 0.2f)
        {
            if(transform.rotation.y == 0)
            {
                transform.Translate(Vector2.left * KnockBackPower * Time.deltaTime * Direction);
            }
            else
            {
                transform.Translate(Vector2.left * KnockBackPower * Time.deltaTime * -1f * Direction);
            }

            KnockBackTime += Time.deltaTime;

            yield return null;
        }

        IsKnockBack = false;
    }

    IEnumerator AlphaBlink()
    {
        while(IsHurt) 
        {
            yield return new WaitForSeconds(0.1f);
            Renderer.color = AlphaA;
            yield return new WaitForSeconds(0.1f);
            Renderer.color = AlphaB;
        }
    }

    IEnumerator SetHurt()
    {
        yield return new WaitForSeconds(0.3f);
        IsHurt = false;
    }
}
