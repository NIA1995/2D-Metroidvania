using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    /* Added Component */
    Animator EnemyAnimator;
    Rigidbody2D RigidBody;
    SpriteRenderer Renderer;

    /* Movement Power */
    public float MoveSpeed;
    public float KnockBackPower;

    /* State Bool */
    bool IsLeftSide;
    bool IsKnockBack = false;
    bool IsDropItem = false;

    /* Status */
    public int HP = 100;
    public int Damage = 10;

    /* Alpha Blink Value */
    Color AlphaA = Color.red;
    Color AlphaB = new Color(1,1,1,1);

    /* Code */
    public int EnemyCode;
    public int Exp;
    public int Gold;

    /* Drop Item Prefab */
    public GameObject Items;

    /* Monster Event */
    //public delegate void MonsterDead();
    //public static event MonsterDead Dead;

    public RespawnManager RespawnManagerInstance;

    public GameObject HudImage;
    
    void Awake()
    {
        EnemyAnimator = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();

        if (Random.Range(0, 2) == 1)
        {
            IsLeftSide = true;
        }
        else
        {
            IsLeftSide = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        StartCoroutine(CommonFunction.FadeIn(gameObject));
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

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if (Collision.tag == "Player")
        {
            Collision.GetComponent<PlayerController>().GetDamage(Damage, gameObject.transform.position);
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
        HP -= Damage;

        GameObject Hud = Instantiate(HudImage);
        Hud.GetComponent<HudText>().Alpha = Color.red;
        Hud.GetComponent<HudText>().TargetString = Damage.ToString();
        Hud.GetComponent<HudText>().transform.position = transform.position;

        if (HP <= 0.0f)
        {
            /* Mosnter Dead */

            IsKnockBack = true;

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;

            StartCoroutine(CommonFunction.FadeOut(gameObject, 0.5f, true));

            if (!IsDropItem)
            {
                IsDropItem = true;

                foreach (var item in PlayerController.Instance.AcceptQuestList)
                {
                    if (item.ObjectCode == EnemyCode)
                    {
                        item.NowTargetCount++;
                    }
                }

                PlayerController.Instance.Gold += Gold;
                PlayerController.Instance.Exp += Exp;

                GameObject NewItem = Instantiate(Items, gameObject.transform.position, Quaternion.identity) as GameObject;
            }

            RespawnManagerInstance.MonsterList.Remove(gameObject);
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

            if(!IsKnockBack)
            {
                StartCoroutine(CommonFunction.AlphaBlink(AlphaA, AlphaB, Renderer));
            }
;        }
    }


    IEnumerator KnockBack(float Direction)
    {
        IsKnockBack = true;

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
}
