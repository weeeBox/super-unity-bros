using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MarioController : EntityController
{
    [SerializeField]
    private float m_JumpHighSpeed = 120.0f;

    [SerializeField]
    private float m_JumpSquashSpeed = 70.0f;

    [SerializeField]
    private float m_WalkAcc = 112.0f;

    [SerializeField]
    private float m_WalkSlowAcc = 57.0f;

    /* User move input */
    private Vector2 m_MoveInput;

    private bool m_Jumping;
    private Vector3 m_InitialPos;

    #region Lifecycle

    protected override void OnAwake()
    {
        base.OnAwake();

        m_InitialPos = transform.localPosition;
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();

        flipX = false;
        m_MoveInput = Vector2.zero;
        m_Jumping = false;
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

    protected override void UpdateAnimation(float deltaTime)
    {
        animator.SetBool("Jump", m_Jumping);
        animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
        animator.SetBool("Stop", m_MoveInput.x > Mathf.Epsilon && m_Velocity.x < 0 || m_MoveInput.x < -Mathf.Epsilon && m_Velocity.x > 0);
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        m_Jumping = false;
    }

    protected override void OnHitBlock(Cell cell)
    {
        map.Jump(cell.i, cell.j);
    }

    protected override void OnDie(bool animated)
    {
        base.OnDie(animated);

        m_MoveInput = Vector2.zero;
        StartCoroutine(DieCoroutine());
    }

    #endregion

    #region Death

    IEnumerator DieCoroutine()
    {
        collisionsEnabled = false;
        movementEnabled = false;

        yield return new WaitForSeconds(0.25f);

        movementEnabled = true;
        m_Velocity.y = m_JumpHighSpeed; // FIXME: use anothe value

        yield return null;
    }

    #endregion

    #region Collisions

    void OnTriggerEnter2D(Collider2D other)
    {
        assert.IsFalse(dead);

        MovingObject obj = other.GetComponent<MovingObject>();
        if (obj != null && !obj.dead)
        {
            if (bottom > obj.bottom)
            {
                OnJumpOnObject(obj);
            }
            else
            {
                OnHitByObject(obj);
            }
        }
    }

    void OnJumpOnObject(MovingObject other)
    {
        EnemyController enemy = other as EnemyController;
        if (enemy != null)
        {
            StartCoroutine(JumpOnEnemy(enemy));
            enemy.Die();
        }
    }

    void OnHitByObject(MovingObject other)
    {
        EnemyController enemy = other as EnemyController;
        if (enemy != null)
        {
            Die();
        }
    }

    IEnumerator JumpOnEnemy(EnemyController enemy)
    {
        float targetY = enemy.posY;

        // player's bottom should be at the center of an enemy
        while (bottom > targetY && !dead)
        {
            yield return null;
        }

        if (!dead)
        {
            m_Velocity.y = m_JumpSquashSpeed; // FIXME: create a convenience method
        }
    }

    #endregion
}