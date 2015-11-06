using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public class MovingObject : BaseBehaviour2D
{
    public const int DIR_LEFT = -1;
    public const int DIR_RIGHT = 1;
    
    [SerializeField]
    Rect m_ColliderRect;

    [SerializeField]
    float m_PushCollisionSpeed = 24.0f;

    protected Vector3 m_Velocity;
    
    ColliderPosition m_LastPosition;
    Collider2D m_Collider;

    int m_Direction;
    bool m_Grounded;
    bool m_Dead;
    bool m_MovementEnabled;
    bool m_CollisionsEnabled;

    #region Lifecycle

    protected override void OnAwake()
    {
        m_Collider = GetRequiredComponent<BoxCollider2D>();
    }

    protected override void OnEnabled()
    {
        m_LastPosition = new ColliderPosition(transform.localPosition, m_ColliderRect);

        m_Collider.enabled = true;
        m_CollisionsEnabled = true;
        m_MovementEnabled = true;
        m_Velocity = Vector3.zero;
        m_Direction = DIR_RIGHT;
        m_Grounded = false;
        m_Dead = false;
    }
    
    protected override void OnFixedUpdate(float deltaTime)
    {
        if (m_MovementEnabled)
        {
            UpdateVelocity(deltaTime);
            UpdatePosition(deltaTime);
        }

        if (m_CollisionsEnabled)
        {
            HandleCollisions();
        }
    }

    #endregion

    #region Inheritance

    protected virtual void UpdateVelocity(float deltaTime)
    {
        m_Velocity.y += Constants.GRAVITY * deltaTime;
    }

    protected virtual void UpdatePosition(float deltaTime)
    {
        m_LastPosition.center = transform.localPosition;
        transform.Translate(m_Velocity.x * deltaTime, m_Velocity.y * deltaTime);
    }

    #endregion

    #region Collisions

    void HandleCollisions()
    {
        float x = this.posX;
        float y = this.posY;
        
        bool grounded = false;

        if (m_Velocity.y > Mathf.Epsilon) // moving up
        {
            Cell cell = GetCell(x, this.top);
            if (cell != null && m_LastPosition.top - cell.bottom < 0.01f) // hit the block
            {
                this.top = cell.bottom;
                m_Velocity.y = 0f;

                OnHitBlock(cell);
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
                    grounded = true;
                }
                else
                {
                    HandleHorCollision(cell, x);
                }
            }
        }

        m_Grounded = grounded;
        if (grounded)
        {
            OnGrounded();
        }
    }
    
    void HandleHorCollision(Cell cell, float x)
    {
        if (x - cell.x > -Mathf.Epsilon)
        {
            this.left = cell.right;
            OnObstacle();
        }
        else
        {
            this.right = cell.left;
            OnObstacle();
        }
    }

    #endregion

    #region Death

    public void Die(bool animated = true)
    {
        assert.IsTrue(!m_Dead);

        m_Dead = true;
        m_Velocity = Vector2.zero;
        m_Collider.enabled = false;

        OnDie(animated);
    }

    #endregion

    #region Callbacks

    protected virtual void OnGrounded()
    {
        m_Velocity.y = 0f;
    }

    protected virtual void OnObstacle()
    {
        m_Velocity.x = 0f;
    }

    protected virtual void OnHitBlock(Cell cell)
    {
    }

    protected virtual void OnDie(bool animated)
    {
    }

    #endregion

    #region Helpers

    protected void Flip()
    {
        m_Direction = -m_Direction;
        flipX = !flipX;
    }
    
    private Cell GetCell(float x, float y)
    {
        return GameManager.map.GetCell(x, y);
    }
    
    private Cell GetCellAt(int i, int j)
    {
        return GameManager.map.GetCellAt(i, j);
    }
    
    #endregion

    #region Properties

    protected Map map
    {
        get { return GameManager.map; }
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

    public bool grounded
    {
        get { return m_Grounded; }
    }

    public int direction
    {
        get { return m_Direction; }
    }

    public bool dead
    {
        get { return m_Dead; }
        protected set { m_Dead = true; }
    }

    public bool collisionsEnabled
    {
        get { return m_CollisionsEnabled; }
        protected set { m_CollisionsEnabled = value; }
    }

    public bool movementEnabled
    {
        get { return m_MovementEnabled; }
        protected set { m_MovementEnabled = value; }
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