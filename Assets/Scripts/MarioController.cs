using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MarioController : EntityController
{
    [SerializeField]
    private float m_JumpHighSpeed = 120.0f;

    [SerializeField]
    private float m_WalkAcc = 112.0f;

    [SerializeField]
    private float m_WalkSlowAcc = 57.0f;

    /* User move input */
    private Vector2 m_MoveInput;

    private bool m_Jumping;
    private bool m_Dead;
    private Vector3 m_InitialPos;

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
        m_Dead = false;
    }

    protected override void UpdateVelocity(float deltaTime)
    {
        base.UpdateVelocity(deltaTime);

        float vx = m_Velocity.x;
        float moveX = m_MoveInput.x;
        
        if (Mathf.Approximately(moveX, 0.0f))
        {
            vx += -m_Direction * m_WalkSlowAcc * deltaTime;
            vx = m_Direction > 0 ? Mathf.Max(vx, 0) : Mathf.Min(vx, 0);
        }
        else
        {
            vx += moveX * m_WalkAcc * deltaTime;
            if (vx > 0)
            {
                vx = Mathf.Min(vx, m_WalkSpeed);
            }
            else if (vx < 0)
            {
                vx = Mathf.Max(vx, -m_WalkSpeed);
            }
        }
        
        if (moveX >  Mathf.Epsilon && m_Direction == DIR_LEFT ||
            moveX < -Mathf.Epsilon && m_Direction == DIR_RIGHT)
        {
            Flip();
        }
        
        m_Velocity.x = vx;
    }

    protected override void UpdateAnimation(float deltaTime)
    {
        m_Animator.SetBool("Jump", m_Jumping);
        m_Animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
        m_Animator.SetBool("Stop", m_MoveInput.x > Mathf.Epsilon && m_Velocity.x < 0 || m_MoveInput.x < -Mathf.Epsilon && m_Velocity.x > 0);
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);

        CheckDead();
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (m_Dead) return;

        m_MoveInput.x = Input.GetAxisRaw("Horizontal");
        m_MoveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded && !m_Jumping)
        {
            m_Jumping = true;
            m_Velocity.y = m_JumpHighSpeed;
        }
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        m_Jumping = false;
    }

    void CheckDead()
    {
        if (!m_Dead && posY < -3.2) // FIXME: remove magic number
        {
            m_Dead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
        transform.position = m_InitialPos;
        gameObject.SetActive(true);
    }
}