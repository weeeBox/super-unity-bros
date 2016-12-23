using UnityEngine;

using System;
using System.Collections;

using LunarCore;

public abstract class LevelObject : BaseBehaviour2D
{
    public const int DIR_LEFT = -1;
    public const int DIR_RIGHT = 1;

    [SerializeField]
    float m_WalkSpeed;

    [SerializeField]
    Rect m_ColliderRect;

    [SerializeField]
    float m_PushCollisionSpeed = 24.0f;

    [SerializeField]
    float m_GravityScale = 1.0f;

    protected Vector3 m_Velocity;
    
    ColliderPosition m_LastPosition;
    Collider2D m_Collider;
    Animator m_Animator;

    /// <summary>
    /// Waiting to become visible. Won't move or participate in collisions if sleeping.
    /// </summary>
    bool m_Sleeping;

    int m_Direction;
    bool m_Grounded;
    bool m_Dead;
    bool m_MovementEnabled = true;
    bool m_MapCollisionsEnabled = true;

    #region MonoBehaviour callbacks

    void Start()
    {
        sleeping = true;
        OnStart();
    }
    
    void FixedUpdate()
    {
        if (m_Sleeping)
        {
            if (left > camera.right) return; // not visible yet
            
            sleeping = false;
            OnBecomeVisible();
        }
        
        OnFixedUpdate(Time.fixedDeltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        assert.IsFalse(sleeping);

        LevelObject obj = other.GetComponent<LevelObject>();
        if (obj != null)
        {
            assert.IsFalse(obj.sleeping);
            OnCollision(obj);
        }
    }

    #endregion

    #region Lifecycle

    protected override void OnAwake()
    {
        Rigidbody2D rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.offset = m_ColliderRect.position;
        collider.size = m_ColliderRect.size;

        m_Collider = collider;
        m_Animator = GetComponent<Animator>();
    }

    protected override void OnEnabled()
    {
        m_LastPosition = new ColliderPosition(transform.localPosition, m_ColliderRect);

        m_Collider.enabled = true;
        m_Velocity = Vector3.zero;
        m_Direction = DIR_RIGHT;
        m_Grounded = false;
    }
    
    protected override void OnFixedUpdate(float deltaTime)
    {
        if (m_MovementEnabled)
        {
            UpdateVelocity(deltaTime);
            UpdatePosition(deltaTime);
        }

        if (m_MapCollisionsEnabled)
        {
            HandleCollisions();
        }
    }

    #endregion

    #region Inheritance

    protected virtual void UpdateVelocity(float deltaTime)
    {
        m_Velocity.y += m_GravityScale * Constants.GRAVITY * deltaTime;
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

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        if (m_Velocity.y > Mathf.Epsilon) // moving up
        {
            Cell cell = GetCell(x, this.top);
            if (cell != null && m_LastPosition.top - cell.bottom < 0.01f) // hit the block
            {
                this.top = cell.bottom;
                m_Velocity.y = 0f;

                OnJumpHitCell(cell);
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

            Cell left = GetCell(this.left + Constants.CELL_EDGE_SMOOTH, this.bottom);
            Cell right = GetCell(this.right - Constants.CELL_EDGE_SMOOTH, this.bottom);
            cell = left != null ? left : right;
            
            if (cell != null)
            {
                if (m_LastPosition.bottom - cell.top > -0.01f) // jumping on the cell
                {
                    this.bottom = cell.top;
                    m_Grounded = true;
                    OnStayGrounded(cell);
                }
                else
                {
                    HandleHorCollision(cell, x);
                }

                if (left != null && left.jumping)
                {
                    OnCellJumped(left);
                }
                else if (right != null && right.jumping)
                {
                    OnCellJumped(right);
                }
            }
        }

        if (wasGrounded && !grounded)
        {
            OnStartFalling();
        }
        else if (!wasGrounded && grounded)
        {
            OnStopFalling();
        }
    }
    
    void HandleHorCollision(Cell cell, float x)
    {
        if (x - cell.x > -Mathf.Epsilon)
        {
            this.left = cell.right;
            OnObstacle(cell);
        }
        else
        {
            this.right = cell.left;
            OnObstacle(cell);
        }
    }

    #endregion

    #region Damage
    
    public void TakeDamage(LevelObject attacker)
    {
        OnDamage(attacker);
    }

    public void Die()
    {
        assert.IsFalse(m_Dead);

        m_Animator.enabled = false;
        collisionsEnabled = false;
        m_Dead = true;
        m_Velocity = Vector2.zero;

        OnDie();
    }
    
    #endregion

    #region Callbacks

    /// <summary>
    /// Called when the object becomes visible
    /// </summary>
    protected virtual void OnBecomeVisible()
    {
    }

    /// <summary>
    /// Called when the object becomes invisible
    /// </summary>
    protected virtual void OnBecomeInvisible()
    {
    }

    /// <summary>
    /// Called when colliding with other object
    /// </summary>
    protected virtual void OnCollision(LevelObject other)
    {
    }

    protected virtual void OnJumpHitCell(Cell cell)
    {
    }

    /// <summary>
    /// Called when player starts falling (first frame not touching ground)
    /// </summary>
    protected virtual void OnStartFalling()
    {
    }

    /// <summary>
    /// Called when player stops falling (first frame touching ground)
    /// </summary>
    protected virtual void OnStopFalling()
    {
    }

    /// <summary>
    /// Called every frame when player stands on the ground
    /// </summary>
    protected virtual void OnStayGrounded(Cell cell)
    {
        m_Velocity.y = 0f;
    }

    protected virtual void OnObstacle(Cell cell)
    {
        FlipHorMovement();
    }

    protected virtual void OnCellJumped(Cell cell)
    {
    }

    protected virtual void OnDamage(LevelObject attacker)
    {
    }

    protected virtual void OnDie()
    {
    }

    #endregion

    #region Movements

    protected void FlipHorMovement()
    {
        m_Velocity.x = -m_Velocity.x;
        Flip();
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

    /// <summary>
    /// True if object is invisible and won't move or participate in collisions
    /// </summary>
    public bool sleeping
    {
        get { return m_Sleeping; }
        set
        {
            m_Sleeping = value;
            objectCollisionsEnabled = !m_Sleeping;
        }
    }

    public float left
    {
        get { return posX + m_ColliderRect.x - 0.5f * m_ColliderRect.width; }
        set { posX = value - m_ColliderRect.x + 0.5f * m_ColliderRect.width; }
    }
    
    public float right
    {
        get { return posX + m_ColliderRect.x + 0.5f * m_ColliderRect.width; }
        set { posX = value - m_ColliderRect.x - 0.5f * m_ColliderRect.width; }
    }
    
    public float top
    {
        get { return posY + m_ColliderRect.y + 0.5f * m_ColliderRect.height; }
        set { posY = value - m_ColliderRect.y - 0.5f * m_ColliderRect.height; }
    }
    
    public float bottom
    {
        get { return posY + m_ColliderRect.y - 0.5f * m_ColliderRect.height; }
        set { posY = value - m_ColliderRect.y + 0.5f * m_ColliderRect.height; }
    }

    public Rect colliderRect
    {
        get { return m_ColliderRect; }
        set
        {
            m_ColliderRect = value;

            BoxCollider2D collider = GetRequiredComponent<BoxCollider2D>();
            collider.offset = m_ColliderRect.position;
            collider.size = m_ColliderRect.size;
        }
    }

    public bool dead
    {
        get { return m_Dead; }
        protected set { m_Dead = value; }
    }

    public float walkSpeed
    {
        get { return m_WalkSpeed; }
    }

    public bool grounded
    {
        get { return m_Grounded; }
    }

    public int direction
    {
        get { return m_Direction; }
        protected set { m_Direction = value; }
    }

    public bool collisionsEnabled
    {
        get { return mapCollisionsEnabled && objectCollisionsEnabled; }
        protected set
        { 
            mapCollisionsEnabled = value;
            objectCollisionsEnabled = value;
        }
    }

    public bool mapCollisionsEnabled
    {
        get { return m_MapCollisionsEnabled; }
        protected set { m_MapCollisionsEnabled = value; }
    }

    public bool objectCollisionsEnabled
    {
        get { return m_Collider.enabled; }
        protected set { m_Collider.enabled = value; }
    }

    public float gravityScale
    {
        get { return m_GravityScale; }
        set { m_GravityScale = value; }
    }

    public bool movementEnabled
    {
        get { return m_MovementEnabled; }
        protected set { m_MovementEnabled = value; }
    }

    protected Animator animator
    {
        get { return m_Animator; }
    }

    protected Map map
    {
        get { return GameManager.map; }
    }
    
    protected new GameCamera camera
    {
        get { return GameManager.camera; }
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