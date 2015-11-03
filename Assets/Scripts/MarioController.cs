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
    private float m_PushCollisionSpeed = 24.0f;

    [SerializeField]
    private float m_WalkAcc = 112.0f;

    [SerializeField]
    private float m_WalkSlowAcc = 57.0f;

    [SerializeField]
    private float m_Gravity = -300.0f;

    [SerializeField]
    private Rect m_ColliderRect;

    /* User move input */
    private Vector2 m_MoveInput;

    private bool m_Jumping;
    private Vector3 m_Velocity;

    private ColliderPosition m_LastPosition;

    private int m_Direction;
    private bool m_Grounded;
    private bool m_Dead;
    private Vector3 m_InitialPos;

    private Animator m_Animator;

    protected override void OnAwake()
    {
        m_Animator = GetRequiredComponent<Animator>();
        m_InitialPos = transform.localPosition;
    }

    protected override void OnEnabled()
    {
        m_LastPosition = new ColliderPosition(transform.localPosition, m_ColliderRect);

        flipX = false;
        m_MoveInput = Vector2.zero;
        m_Jumping = false;
        m_Velocity = Vector3.zero;
        m_Direction = DIR_RIGHT;
        m_Grounded = false;
        m_Dead = false;
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

        vy += m_Gravity * deltaTime;

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
        CheckDead();

        m_Animator.SetBool("Jump", m_Jumping);
        m_Animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
        m_Animator.SetBool("Stop", moveX > Mathf.Epsilon && m_Velocity.x < 0 || moveX < -Mathf.Epsilon && m_Velocity.x > 0);
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

    void HandleCollisions()
    {
        float x = this.posX;
        float y = this.posY;
        float vx = m_Velocity.x;
        float vy = m_Velocity.y;

        if (vy > Mathf.Epsilon) // moving up
        {
            Cell cell = GetCell(x, this.top);
            if (cell != null && m_LastPosition.top - cell.bottom < 0.01f) // hit the block
            {
                this.top = cell.bottom;
                m_Velocity.y = 0f;
            }
            else
            {
                cell = GetCell(this.left, this.top);
                cell = cell != null ? cell : GetCell(this.right, this.top);

                if (cell != null)
                {
                    if (m_LastPosition.top - cell.bottom < 0.01f) // hit from the bottom?
                    {
                        float dist = x - cell.x;
                        float sign = dist < 0f ? -1f : 1f;
                        float penetration = 0.5f * (Constants.CELL_WIDTH + m_ColliderRect.width) - Mathf.Abs(dist);
                        float move = sign * Mathf.Min(m_PushCollisionSpeed * Time.fixedDeltaTime, penetration);
                        
                        transform.Translate(move, 0f);
                    }
                    else
                    {
                        HandleHorCollision(cell, x);
                    }
                }
                else
                {
                    cell = GetCell(this.left, y);
                    cell = cell != null ? cell : GetCell(this.right, y);

                    if (cell != null)
                    {
                        HandleHorCollision(cell, x);
                    }
                }
            }
        }
        else // moving down
        {
            Cell cell = GetCell(this.left, y);
            cell = cell != null ? cell : GetCell(this.right, y);
            
            if (cell != null)
            {
                HandleHorCollision(cell, x);
            }

            cell = GetCell(this.left, this.bottom);
            cell = cell != null ? cell : GetCell(this.right, this.bottom);

            if (cell != null)
            {
                if (m_LastPosition.bottom - cell.top > -0.01f) // jumping on the cell
                {
                    this.bottom = cell.top;
                    
                    m_Grounded = true;
                    m_Jumping = false;
                    m_Velocity.y = 0f;
                }
                else
                {
                    HandleHorCollision(cell, x);
                }
            }
        }
    }

    private void HandleHorCollision(Cell cell, float x)
    {
        if (x - cell.x > -Mathf.Epsilon)
        {
            this.left = cell.right;
            m_Velocity.x = 0.0f;
        }
        else
        {
            this.right = cell.left;
            m_Velocity.x = 0.0f;
        }
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

    private void Flip()
    {
        m_Direction = -m_Direction;
        flipX = !flipX;
    }

    #region Helpers

    private Cell GetCell(float x, float y)
    {
        return GameManager.map.GetCell(x, y);
    }

    private Cell GetCellAt(int i, int j)
    {
        return GameManager.map.GetCellAt(i, j);
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

        public float x
        {
            get { return center.x; }
        }

        public float y
        {
            get { return center.y; }
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