using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MarioController : BaseBehaviour2D
{
    private const int DIR_LEFT = -1;
    private const int DIR_RIGHT = 1;

    [SerializeField]
    private float m_JumpHighSpeed = 120.0f;

    [SerializeField]
    private float m_WalkSpeed = 36.0f;

    [SerializeField]
    private float m_WalkAcc = 112.0f;

    [SerializeField]
    private float m_WalkSlowAcc = 57.0f;

    [SerializeField]
    private Transform m_GroundCheck;

    [SerializeField]
    private LayerMask m_GroundCheckMask;

    private Collider2D[] m_GroundCheckResult;
    private bool m_Jumping;
    private Vector2 m_Velocity;
    private int m_Direction;
    private bool m_Grounded;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;

    protected override void OnAwake()
    {
        Assert.IsNotNull(m_GroundCheck);

        m_Animator = GetRequiredComponent<Animator>();
        m_Rigidbody = GetRequiredComponent<Rigidbody2D>();
        
        m_Direction = DIR_RIGHT;
        m_GroundCheckResult = new Collider2D[1];
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = Physics2D.OverlapCircleNonAlloc(m_GroundCheck.position, 0.2f, m_GroundCheckResult, m_GroundCheckMask) > 0;
        if (!wasGrounded && m_Grounded && m_Jumping)
        {
            m_Jumping = false;
            m_Animator.SetBool("Jump", false);
        }

        float move = Input.GetAxisRaw("Horizontal");

        float vx = m_Rigidbody.velocity.x;

        if (Mathf.Approximately(move, 0.0f))
        {
            vx += -m_Direction * m_WalkSlowAcc * deltaTime;
            vx = m_Direction > 0 ? Mathf.Max(vx, 0) : Mathf.Min(vx, 0);
        }
        else
        {
            vx += move * m_WalkAcc * deltaTime;
            if (vx > 0)
            {
                vx = Mathf.Min(vx, m_WalkSpeed);
            }
            else if (vx < 0)
            {
                vx = Mathf.Max(vx, -m_WalkSpeed);
            }
        }

        if (move >  Mathf.Epsilon && m_Direction == DIR_LEFT ||
            move < -Mathf.Epsilon && m_Direction == DIR_RIGHT)
        {
            Flip();
        }

        if (m_Jumping)
        {
            m_Animator.SetBool("Jump", true);
        }
        else
        {
            m_Animator.SetFloat("Speed", Mathf.Abs(vx));
        }

        m_Animator.SetBool("Stop", move > Mathf.Epsilon && vx < 0 || move < -Mathf.Epsilon && vx > 0);
        m_Rigidbody.velocity = new Vector2(vx, m_Rigidbody.velocity.y);
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded && !m_Jumping)
        {
            m_Jumping = true;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpHighSpeed);
        }
    }

    private void Flip()
    {
        m_Direction = -m_Direction;
        flipX = !flipX;
    }
}
