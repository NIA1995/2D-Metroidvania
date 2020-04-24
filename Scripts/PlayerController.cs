using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

/* 20.04.19 최종 수정 */
/* IsGrounded 수정이 필요함, 높은 곳에서 떨어질 때 IsGrounded는 여전히 True임 */

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    /* Added Component */
    Animator PlayerAnimator;
    Rigidbody2D RigidBody;
    SpriteRenderer Renderer;
    Collider2D MyCollider;

    /* Movement Power */
    public float MovePower = 3f;
    public float JumpPower = 5f;
    public float KnockBackPower = 2f;

    /* State Bool */
    public bool IsJumping = false;
    public bool IsGrounded = true;
    public bool IsMovingEnable = true;
    public bool IsAttack = false;
    public bool IsHurt = false;
    public bool IsKnockBack = false;
    public bool IsChat = false;

    /* Animation */
    float IdleBlendValue = 0.0f;
    float AnimationExitTime = 0.85f;
    float AttackBlendValue = 0.0f;

    /* Attack */
    public Transform AttackPostiion;
    public Vector2 AttackBoxSize;

    /* Status */
    public int Level = 1;
    public int HP = 300;
    public int Damage = 35;

    public int Exp = 0;
    public int MaxExp = 100;

    public int Gold = 0;

    /* Alpha Blink Value */
    Color AlphaA = Color.red;
    Color AlphaB = new Color(1, 1, 1, 1);

    /* Quest System */
    public List<QuestData> AcceptQuestList;

    /* Component Allocation */
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        PlayerAnimator = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        MyCollider = GetComponent<Collider2D>();

        ChatData.Chat += SetChatBool;
    }
    
    void SetChatBool()
    {
        if(!IsChat)
        {
            IsChat = true;
        }
        else
        {
            IsChat = false;
        }
    }

    /* Animation Function */
    IEnumerator IdleAnimationState(string AnimationName)
    {
        while (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName))
        {
            if (PlayerAnimator.GetInteger("IsRunning") == 1)
            {
                StopCoroutine(IdleAnimationState(AnimationName));
            }

            /* 애니메이션 전환 중 실행되는 부분 */
            yield return null;
        }

        while (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < AnimationExitTime)
        {
            if (PlayerAnimator.GetInteger("IsRunning") == 1)
            {
                StopCoroutine(IdleAnimationState(AnimationName));
            }

            /* 애니메이션 진행 중 실행되는 부분 */
            yield return null;
        }

        /* 애니메이션 완료 후 실행되는 부분 */
        IdleBlendValue = UnityEngine.Random.Range(0, 2);
        PlayerAnimator.SetFloat("IdleBlendValue", IdleBlendValue);
    }

    IEnumerator AttackAnimationState(string AnimationName)
    {
        while (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(AnimationName))
        {
            /* 애니메이션 전환 중 실행되는 부분 */
            yield return null;
        }

        while (PlayerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < AnimationExitTime)
        {
            /* 애니메이션 진행 중 실행되는 부분 */
            yield return null;
        }

        /* 애니메이션 완료 후 실행되는 부분 */
        PlayerAnimator.SetBool("IsHit", false);
        AttackBlendValue++;
    }

    public void PlayBlendAnimation(float BlendNumber, string BlendName, string TriggerName)
    {
        PlayerAnimator.SetFloat(BlendName, BlendNumber);
        PlayerAnimator.SetBool(TriggerName, true);
    }

    /* Attack Function */
    public void SetAttack()
    {
        StartCoroutine(ComboAttack());
    }

    IEnumerator ComboAttack()
    {
        yield return null;

        if(!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            PlayBlendAnimation(AttackBlendValue, "AttackBlendValue", "IsHit");
            StartCoroutine(AttackAnimationState("Attack"));

            Collider2D[] Colliders = Physics2D.OverlapBoxAll(AttackPostiion.position, AttackBoxSize, 0);

            foreach (Collider2D Collider in Colliders)
            {
                if(Collider.tag == "Enemy")
                {
                    if(AttackBlendValue < 2)
                    {
                        Collider.GetComponent<EnemyController>().GetDamage(Damage, MyCollider.transform.position, false);
                    }
                    else
                    {
                        Collider.GetComponent<EnemyController>().GetDamage(Damage, MyCollider.transform.position, true);
                    }
                }
            }

            if(AttackBlendValue > 1)
            {
                AttackBlendValue = -1;
            }
        }

    }

    private void SetAttackPositionDirection(string Direction)
    {
        switch (Direction)
        {
            case "Left": AttackPostiion.gameObject.transform.localPosition = (new Vector3(-0.13f, -0.05f, 0f)); break;
            case "Right": AttackPostiion.gameObject.transform.localPosition = (new Vector3(0.13f, -0.05f, 0f)); break;
        }
    }


    public void GetDamage(int Damage, Vector2 Pos)
    {
        if(!IsHurt)
        {
            IsHurt = true;
            HP -= Damage;

            if (HP <= 0.0f)
            {
                /* GameOver */
                Destroy(gameObject);
            }
            else
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
                StartCoroutine(CommonFunction.AlphaBlink(AlphaA, AlphaB, Renderer));
                StartCoroutine(SetHurt());
            }
        }
    }

    IEnumerator KnockBack(float Direction)
    {
        IsKnockBack = true;

        float KnockBackTime = 0.0f;

        while (KnockBackTime < 0.2f)
        {
            if (transform.rotation.y == 0)
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

    IEnumerator SetHurt()
    {
        yield return new WaitForSeconds(2.0f);
        IsHurt = false;
    }

    /* Play Routine */
    void Update()
    {
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            IsAttack = true;
            IsMovingEnable = false;
        }

        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run") ||
            PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleJump") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("IsRunJump"))
        {
            IsAttack = false;
            IsMovingEnable = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !IsAttack && !IsChat)
        {
            SetAttack();
        }

        if (IsMovingEnable)
        {
            if(IsGrounded)
            {
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    PlayerAnimator.SetInteger("IsRunning", 0);

                    if (!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        StartCoroutine(IdleAnimationState("Idle"));
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    PlayerAnimator.SetInteger("IsRunning", 1);
                    AttackBlendValue = 0;
                    SetAttackPositionDirection("Left");
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    PlayerAnimator.SetInteger("IsRunning", 1);
                    AttackBlendValue = 0;
                    SetAttackPositionDirection("Right");
                }

                if (Input.GetKeyDown(KeyCode.X) && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    IsJumping = true;
                    Jump();
                    PlayerAnimator.SetBool("IsJump", true);
                }

                if (Input.GetKeyDown(KeyCode.X) && PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    IsJumping = true;
                    Jump();
                    PlayerAnimator.SetBool("IsRunJump", true);
                }
            }           
        }
    }

    private void FixedUpdate()
    {
        if (IsMovingEnable)
        {
            CharacterMovement();
            Jump();
        }
    }


    void CharacterMovement()
    {
        Vector3 MoveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            MoveVelocity = Vector3.left;
            Renderer.flipX = true;

        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            MoveVelocity = Vector3.right;
            Renderer.flipX = false;

        }

        transform.position += MoveVelocity * MovePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!IsJumping)
            return;

        if (IsGrounded == true)
        {
            IsGrounded = false;
            RigidBody.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, JumpPower);
            RigidBody.AddForce(jumpVelocity, ForceMode2D.Impulse);
            IsJumping = false;
        }

    }


    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            PlayerAnimator.SetBool("IsJump", false);
            PlayerAnimator.SetBool("IsRunJump", false);
        }
    }

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if(Collision.gameObject.tag == "Items")
        {
            if(Input.GetKey(KeyCode.C))
            {
                /* 아이템과 상호작용 추가 */

                Destroy(Collision.gameObject);
            }
        }
    }

    void OnCollisionExit2D(Collision2D Collision)
    {
        if(Collision.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackPostiion.position, AttackBoxSize);
    }
}
