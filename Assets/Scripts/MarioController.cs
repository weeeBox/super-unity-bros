using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MarioController : BaseBehaviour2D, IMapCollider
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
    private Rect m_ColliderRect;

    private Vector2 m_MoveInput;

    private bool m_Jumping;
    private Vector3 m_Velocity;

    private ColliderPosition m_Position;
    private ColliderPosition m_LastPosition;

    private int m_Direction;
    private bool m_Grounded;

    private Animator m_Animator;

    protected override void OnAwake()
    {
        m_Animator = GetRequiredComponent<Animator>();
        m_Direction = DIR_RIGHT;
    }

    protected override void OnEnabled()
    {
        m_LastPosition = new ColliderPosition(transform.localPosition, m_ColliderRect);
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        float vx = m_Velocity.x;
        float vy = m_Velocity.y;
        float moveX = m_MoveInput.x;
        float moveY = m_MoveInput.y;

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

        vy += -300 * deltaTime;

        if (moveX >  Mathf.Epsilon && m_Direction == DIR_LEFT ||
            moveX < -Mathf.Epsilon && m_Direction == DIR_RIGHT)
        {
            Flip();
        }

        m_Velocity.x = vx;
        m_Velocity.y = vy;
        m_LastPosition.center = transform.localPosition;

        transform.Translate(m_Velocity.x * deltaTime, m_Velocity.y * deltaTime);

        m_Grounded = false;

        HandleCollisions();

        m_Animator.SetBool("Jump", m_Jumping);
        m_Animator.SetFloat("Speed", Mathf.Abs(vx));
        m_Animator.SetBool("Stop", moveX > Mathf.Epsilon && vx < 0 || moveX < -Mathf.Epsilon && vx > 0);
    }

    protected override void OnUpdate(float deltaTime)
    {
        m_MoveInput.x = Input.GetAxisRaw("Horizontal");
        m_MoveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded && !m_Jumping)
        {
            m_Jumping = true;
            m_Velocity.y = m_JumpHighSpeed;
        }
    }

    void HandleCollisions()
    {
        GameManager.map.HandleCollisions(this);
    }

    private void Flip()
    {
        m_Direction = -m_Direction;
        flipX = !flipX;
    }

    #region IMapCollider implementation

    public void OnCollision(Cell cell)
    {
        float x = this.posX;
        float y = this.posY;
        float lastX = m_LastPosition.center.x;
        float lastY = m_LastPosition.center.y;
        float vx = m_Velocity.x;
        float vy = m_Velocity.y;

        bool hitVerticalObstacle = false;

        if (y > cell.y) // player's center is higher than cell's center
        {
            if (m_LastPosition.bottom - cell.top > -0.01f) // player was higher than cell or standing on it
            {
                this.bottom = cell.top;
                m_Grounded = true;
                m_Jumping = false;

                hitVerticalObstacle = true;
            }
        }
        else if (y < cell.y) // player's center is lower than cell's center
        {
            if (m_LastPosition.top - cell.bottom < 0.01f) // player was lower than cell or right under if
            {
                this.top = cell.bottom;
                hitVerticalObstacle = true;
            }
        }

        if (x > cell.x)
        {
            if (m_LastPosition.left - cell.right > - 0.01f && !hitVerticalObstacle)
            {
                this.left = cell.right;
                m_Velocity.x = 0.0f;
            }
        }
        else if (x < cell.x)
        {
            if (m_LastPosition.right - cell.left < 0.01f && !hitVerticalObstacle)
            {
                this.right = cell.left;
                m_Velocity.x = 0.0f;
            }
        }

        if (hitVerticalObstacle)
        {
            m_Velocity.y = 0.0f;
        }
    }

    public Rect colliderRect
    {
        get
        {
            Rect rect = m_ColliderRect;
            rect.center = transform.localPosition;
            return rect;
        }
    }

    public float left
    {
        get { return posX - 0.5f * m_ColliderRect.width; }
        set { posX = value + 0.5f * m_ColliderRect.width; }
    }

    public float right
    {
        get { return posX + 0.5f * m_ColliderRect.width; }
        set { posX = value - 0.5f * m_ColliderRect.width; }
    }

    public float top
    {
        get { return posY + 0.5f * m_ColliderRect.height; }
        set { posY = value - 0.5f * m_ColliderRect.height; }
    }

    public float bottom
    {
        get { return posY - 0.5f * m_ColliderRect.height; }
        set { posY = value + 0.5f * m_ColliderRect.height; }
    }

    #endregion

    struct ColliderPosition
    {
        public Vector3 center;
        Vector2 colliderHalfSize;

        public ColliderPosition(Vector3 center, Rect colliderRect)
        {
            this.center = center;
            this.colliderHalfSize = new Vector2(0.5f * colliderRect.width, 0.5f * colliderRect.height);
        }

        public float left
        {
            get { return center.x - colliderHalfSize.x; }
        }

        public float right
        {
            get { return center.x + colliderHalfSize.x; }
        }

        public float top
        {
            get { return center.y + colliderHalfSize.y; }
        }
        
        public float bottom
        {
            get { return center.y - colliderHalfSize.y; }
        }
    }
}