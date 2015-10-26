using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MarioController : BaseBehaviour2D
{
    private enum Direction { LEFT, RIGHT };

    [SerializeField]
    private float m_JumpHighSpeed = 12.0f;

    [SerializeField]
    private float m_WalkSpeed = 3.6f;

    [SerializeField]
    private float m_Gravity = -30.0f;

    private bool m_Jumping;
    private Direction m_Direction;
    private Vector2 m_Velocity;

    private Animator m_Animator;
    private Rigidbody2D m_rigidbody;

    private Vector2 m_ground; // FIXME: remove that

    protected override void OnAwake()
    {
        m_Animator = GetRequiredComponent<Animator>();
        m_rigidbody = GetRequiredComponent<Rigidbody2D>();

        m_Direction = Direction.RIGHT;

        m_ground = this.position2D;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        float move = Input.GetAxisRaw("Horizontal");

        float vx = move * m_WalkSpeed;
        float vy = m_rigidbody.velocity.y;

        if (Input.GetKeyDown(KeyCode.Space) && !m_Jumping)
        {
//            m_Jumping = true;
//            m_Animator.SetBool("Jump", m_Jumping);
            vy = m_JumpHighSpeed;
        }
        else
        {
            m_Animator.SetFloat("Speed", Mathf.Abs(vx));
        }

        m_rigidbody.velocity = new Vector2(vx, vy);

        // m_Velocity.y += m_Gravity * deltaTime;


//
//
//        float speed = Math.Abs(horizontalAxis) * m_Speed;
//
//        m_Animator.SetFloat("Speed", Math.Abs(horizontalAxis));
    }
}
