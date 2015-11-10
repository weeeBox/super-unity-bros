using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class PlayerController : LevelObject
{
    enum State
    {
        Small = 0,
        Big,
        Super
    }

    [SerializeField]
    private float m_JumpHighSpeed = 120.0f;

    [SerializeField]
    private float m_JumpSquashSpeed = 70.0f;

    [SerializeField]
    private float m_WalkAcc = 112.0f;

    [SerializeField]
    private float m_WalkSlowAcc = 57.0f;

    [SerializeField]
    private RuntimeAnimatorController m_BigAnimatorController;

    State m_State;

    /* User move input */
    Vector2 m_MoveInput;

    bool m_Jumping;

    RuntimeAnimatorController m_InitialAnimatorController;

    #region Lifecycle

    protected override void OnAwake()
    {
        base.OnAwake();

        assert.IsNotNull(m_BigAnimatorController);

        m_InitialAnimatorController = animator.runtimeAnimatorController;
        m_State = State.Small;
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();

        flipX = false;
        m_MoveInput = Vector2.zero;
        m_Jumping = false;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);
        UpdateAnimation();
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (dead) return;

        m_MoveInput.x = Input.GetAxisRaw("Horizontal");
        m_MoveInput.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space) && grounded && !m_Jumping)
        {
            m_Jumping = true;
            m_Velocity.y = m_JumpHighSpeed;
        }
    }

    #endregion

    #region Inheritance

    protected override void UpdateVelocity(float deltaTime)
    {
        base.UpdateVelocity(deltaTime);

        float vx = m_Velocity.x;
        float moveX = m_MoveInput.x;
        
        if (Mathf.Approximately(moveX, 0.0f))
        {
            vx += -direction * m_WalkSlowAcc * deltaTime;
            vx = direction > 0 ? Mathf.Max(vx, 0) : Mathf.Min(vx, 0);
        }
        else
        {
            vx += moveX * m_WalkAcc * deltaTime;
            if (vx > 0)
            {
                vx = Mathf.Min(vx, walkSpeed);
            }
            else if (vx < 0)
            {
                vx = Mathf.Max(vx, -walkSpeed);
            }
        }
        
        if (moveX >  Mathf.Epsilon && direction == DIR_LEFT ||
            moveX < -Mathf.Epsilon && direction == DIR_RIGHT)
        {
            Flip();
        }
        
        m_Velocity.x = vx;
    }

    protected override void UpdatePosition(float deltaTime)
    {
        base.UpdatePosition(deltaTime);

        // keep player visible
        if (left < camera.left)
        {
            left = camera.left;
            m_Velocity.x = 0;
        }
        else if (right > camera.right)
        {
            right = camera.right;
            m_Velocity.x = 0;
        }
    }

    void UpdateAnimation()
    {
        if (!dead)
        {
            animator.SetBool("Jump", m_Jumping);
            animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
            animator.SetBool("Stop", m_MoveInput.x > Mathf.Epsilon && m_Velocity.x < 0 || m_MoveInput.x < -Mathf.Epsilon && m_Velocity.x > 0);
        }
    }

    protected override void OnGrounded(Cell cell)
    {
        base.OnGrounded(cell);

        m_Jumping = false;
    }

    protected override void OnObstacle(Cell cell)
    {
        m_Velocity.x = 0f;
    }

    protected override void OnJumpHitCell(Cell cell)
    {
        cell.Hit(this);
    }

    protected override void OnDie()
    {
        m_MoveInput = Vector2.zero;
        StartCoroutine(DieCoroutine());
    }

    #endregion

    #region State

    public void AdvanceState()
    {
        if (m_State < State.Super)
        {
            SetState(m_State + 1);
        }
    }

    void SetState(State state)
    {
        switch (state)
        {
            case State.Small:
                animator.runtimeAnimatorController = m_InitialAnimatorController;
                break;
            case State.Big:
                animator.runtimeAnimatorController = m_BigAnimatorController;
                break;
            case State.Super:
                animator.runtimeAnimatorController = m_BigAnimatorController;
                break;
        }

        animator.SetTrigger("ChangeState");

        m_State = state;
    }

    #endregion

    #region Death

    IEnumerator DieCoroutine()
    {
        animator.SetBool("Stop", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Dead", true);

        movementEnabled = false;

        yield return new WaitForSeconds(0.25f);

        movementEnabled = true;
        m_Velocity.y = m_JumpHighSpeed; // FIXME: use anothe value

        yield return null;
    }

    #endregion

    #region Collisions

    protected override void OnCollision(LevelObject other)
    {
        if (other.dead) return;

        if (bottom > other.bottom)
        {
            OnJumpOnObject(other);
        }
        else
        {
            OnCollideObject(other);
        }
    }

    void OnJumpOnObject(LevelObject other)
    {
        EnemyController enemy = other as EnemyController;
        if (enemy != null)
        {
            enemy.OnPlayerJump(this);
            return;
        }

        Powerup powerup = other as Powerup;
        if (powerup != null)
        {
            PickPowerup(powerup);
        }
    }

    void OnCollideObject(LevelObject other)
    {
        EnemyController enemy = other as EnemyController;
        if (enemy != null)
        {
            enemy.OnPlayerCollision(this);
            return;
        }

        Powerup powerup = other as Powerup;
        if (powerup != null)
        {
            PickPowerup(powerup);
        }
    }

    void PickPowerup(Powerup powerup)
    {
        powerup.Apply(this);
        Destroy(powerup.gameObject);
    }

    #endregion

    #region Damage

    protected override void OnDamage(LevelObject attacker)
    {
        Die();
    }

    #endregion

    #region Enemies

    public void JumpOnEnemy(EnemyController enemy)
    {
        StartCoroutine(JumpOnEnemyCoroutine(enemy));
    }

    IEnumerator JumpOnEnemyCoroutine(EnemyController enemy)
    {
        float bottomTargetY = enemy.posY;
        
        // player's bottom should be at the center of an enemy
        while (bottom > bottomTargetY)
        {
            yield return null;
        }
        
        bottom = bottomTargetY;

        m_Jumping = true;
        m_Velocity.y = m_JumpSquashSpeed; // FIXME: create a convenience method
    }

    #endregion
}