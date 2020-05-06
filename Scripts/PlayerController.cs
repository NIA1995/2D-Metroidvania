using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

/* 20.05.05 최종 수정 */
/* IsGrounded 수정이 필요함, 높은 곳에서 떨어질 때 IsGrounded는 여전히 True임 */
/* ComboAttack 함수에서 마지막 공격 넉백 기능 함수를 분리해주는게 좋을까? */

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
    public bool IsLeft = false;
    public bool IsLadder = false;

    /* Animation */
    float IdleBlendValue = 0.0f;
    float AnimationExitTime = 0.85f;
    float AttackBlendValue = 0.0f;

    /* Attack Collider */
    public Transform AttackPostiion;
    public Vector2 AttackBoxSize;

    /* Status */
    public int HP = 5;
    public int MaxHP = 5;

    public int Damage = 2;

    public int Gold = 0;

    /* Alpha Blink Value */
    Color AlphaA = Color.red;
    Color AlphaB = new Color(1, 1, 1, 1);

    /* Quest System */
    public List<QuestSystem> AcceptQuestList;

    /* Used Prefabs */
    public GameObject ArrowPrefab;


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
        PlayerAnimator.SetBool(AnimationName, false);

        if(AnimationName == "Attack")
        {
            AttackBlendValue++;
        }

        Invoke(AnimationName, 0f);
    }

    public void PlayBlendAnimation(float BlendNumber, string BlendName, string TriggerName)
    {
        PlayerAnimator.SetFloat(BlendName, BlendNumber);
        PlayerAnimator.SetBool(TriggerName, true);
    }

    /* Attack Function */
    IEnumerator ComboAttack()
    {
        yield return null;

        if(!PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            PlayBlendAnimation(AttackBlendValue, "AttackBlendValue", "Attack");
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
                float Dircetion = transform.position.x - Pos.x;

                if (Dircetion < 0)
                {
                    Dircetion = 1;
                }
                else
                {
                    Dircetion = -1;
                }

                StartCoroutine(KnockBack(Dircetion));
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
        if (IsMovingEnable)
        {
            /* 사다리 타기 */
            if (IsLadder)
            {
                RigidBody.gravityScale = 0;

                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    PlayerAnimator.SetInteger("IsRunning", 0);
                }

                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    transform.position += Vector3.up * MovePower * Time.deltaTime;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    transform.position += Vector3.down * MovePower * Time.deltaTime;
                }
            }
            else if (!IsLadder)
            {
                RigidBody.gravityScale = 1;
            }

            if (IsGrounded)
            {
                /* 구르기 */
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    PlayerAnimator.SetBool("Slide", true);

                    MovePower = 5f;

                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

                    StartCoroutine(AttackAnimationState("Slide"));
                }

                /* 점프 */
                if(Input.GetKeyDown(KeyCode.X))
                {
                    IsJumping = true;
                    Jump();

                    if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        PlayerAnimator.SetBool("IsJump", true);
                    }
                    else if(PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                    {
                        PlayerAnimator.SetBool("IsRunJump", true);
                    }
                }

                /* 원거리 공격 */
                if (Input.GetKeyDown(KeyCode.D) && !IsAttack && !IsChat)
                {
                    if (IsLeft && !IsAttack)
                    {
                        PlayerAnimator.SetBool("Arrow", true);

                        StartCoroutine(NewArrow(transform.position, -1, 10, 0.25f));
                    }
                    else if (!IsLeft && !IsAttack)
                    {
                        PlayerAnimator.SetBool("Arrow", true);

                        StartCoroutine(NewArrow(transform.position, 1, 10, 0.25f));
                    }

                    StartCoroutine(AttackAnimationState("Arrow"));
                }
            }           
        }

        /* 상태 체크 후 Bool 값 변경 */
        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Arrow"))
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

        /* 근접 공격 */
        if (Input.GetKeyDown(KeyCode.Z) && !IsAttack && !IsChat)
        {
            StartCoroutine(ComboAttack());
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
        if(!IsLadder)
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

            /* 좌우 이동 */
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
                IsLeft = true;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                PlayerAnimator.SetInteger("IsRunning", 1);
                AttackBlendValue = 0;
                SetAttackPositionDirection("Right");
                IsLeft = false;
            }

            transform.position += MoveVelocity * MovePower * Time.deltaTime;
        }     
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

    void Slide()
    {
        MovePower = 3f;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
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

        }

        if (Collision.CompareTag("Ladder"))
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                IsLadder = true;
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

    IEnumerator NewArrow(Vector3 StartPos, int Direction, int Power, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        GameObject NewArrow = Instantiate(ArrowPrefab);
        NewArrow.GetComponent<Arrow>().Set(StartPos, Direction, Power);
    }

    private void OnTriggerExit2D(Collider2D Collision)
    {
        if(Collision.CompareTag("Ladder"))
        {
            IsLadder = false;
        }
    }

    private void Attack()
    {

    }

    private void Arrow()
    {

    }
}
