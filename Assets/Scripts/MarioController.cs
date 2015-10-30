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
    private Vector3 m_Velocity;
    private int m_Direction;
    private bool m_Grounded;

    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_Collider;
    private Vector2 m_ColliderSize;

    protected override void OnAwake()
    {
        assert.IsNotNull(m_GroundCheck);

        m_Animator = GetRequiredComponent<Animator>();
        m_Rigidbody = GetRequiredComponent<Rigidbody2D>();
        m_Collider = GetRequiredComponent<BoxCollider2D>();
        
        m_Direction = DIR_RIGHT;
        m_GroundCheckResult = new Collider2D[1];

        m_ColliderSize = m_Collider.size;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
//        bool wasGrounded = m_Grounded;
//        m_Grounded = Physics2D.OverlapCircleNonAlloc(m_GroundCheck.position, 0.2f, m_GroundCheckResult, m_GroundCheckMask) > 0;
//        if (!wasGrounded && m_Grounded && m_Jumping)
//        {
//            m_Jumping = false;
//            m_Animator.SetBool("Jump", false);
//        }

        float moveX = Input.GetAxisRaw("Horizontal");
		float moveY = Input.GetAxisRaw("Vertical");

//        float vx = m_Velocity.x;
//        float vy = m_Velocity.y;
//
//        if (Mathf.Approximately(moveX, 0.0f))
//        {
//            vx += -m_Direction * m_WalkSlowAcc * deltaTime;
//            vx = m_Direction > 0 ? Mathf.Max(vx, 0) : Mathf.Min(vx, 0);
//        }
//        else
//        {
//            vx += moveX * m_WalkAcc * deltaTime;
//            if (vx > 0)
//            {
//                vx = Mathf.Min(vx, m_WalkSpeed);
//            }
//            else if (vx < 0)
//            {
//                vx = Mathf.Max(vx, -m_WalkSpeed);
//            }
//        }
//
//		vy += -300 * deltaTime;
//
//        if (moveX >  Mathf.Epsilon && m_Direction == DIR_LEFT ||
//            moveX < -Mathf.Epsilon && m_Direction == DIR_RIGHT)
//        {
//            Flip();
//        }
//
//        if (m_Jumping)
//        {
//            m_Animator.SetBool("Jump", true);
//        }
//        else
//        {
//            m_Animator.SetFloat("Speed", Mathf.Abs(vx));
//        }

//        m_Velocity = new Vector2(vx, vy);
        Vector2 position = transform.localPosition + new Vector3(moveX * m_WalkSpeed * deltaTime, moveY * m_WalkSpeed * deltaTime);

        CheckCollisions(ref position);

        transform.localPosition = position;
        // m_Animator.SetBool("Stop", moveX > Mathf.Epsilon && vx < 0 || moveX < -Mathf.Epsilon && vx > 0);
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded && !m_Jumping)
        {
            m_Jumping = true;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpHighSpeed);
        }
    }

    void CheckCollisions(ref Vector2 position)
    {
        Map map = GameManager.map;

        float minX = position.x - 0.5f * m_ColliderSize.x;
		float maxX = position.x + 0.5f * m_ColliderSize.x;
		float minY = position.y - 0.5f * m_ColliderSize.y;
		float maxY = position.y + 0.5f * m_ColliderSize.y;

        Cell c1 = map.GetCell(minX, maxY);
        Cell c2 = map.GetCell(maxX, maxY);

        if (c1 != null)
        {
            position.y = c1.y + 0.5f * (Constants.CELL_HEIGHT + m_ColliderSize.y);
        }
        else if (c2 != null)
        {
            position.y = c2.y + 0.5f * (Constants.CELL_HEIGHT + m_ColliderSize.y);
        }
    }

    private bool CollidesCell(Cell cell)
    {
        if (cell == null) return false;

        Vector2 size = m_Collider.size;
        Vector2 pos = m_Collider.bounds.center;

        return Math.Abs(pos.x - cell.x) < 0.5f * (size.x + Constants.CELL_WIDTH) && 
               Math.Abs(pos.y - cell.y) < 0.5f * (size.y + Constants.CELL_HEIGHT);
    }

    private void Flip()
    {
        m_Direction = -m_Direction;
        flipX = !flipX;
    }

    private float minX
    {
        get { return transform.localPosition.x - 0.5f * m_ColliderSize.x; }
    }

    private float minY
    {
        get { return transform.localPosition.y - 0.5f * m_ColliderSize.y; }
    }

    private float maxX
    {
        get { return transform.localPosition.x + 0.5f * m_ColliderSize.x; }
    }

    private float maxY
    {
        get { return transform.localPosition.y + 0.5f * m_ColliderSize.y; }
    }
}
