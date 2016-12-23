using UnityEngine;

using System;
using System.Collections;

using LunarCore;

[Serializable]
public class PlayerShotInfo
{
    [SerializeField]
    FireBallController m_prefab;

    [SerializeField]
    Transform m_origin;

    public FireBallController prefab
    {
        get { return m_prefab; }
    }

    public Transform origin
    {
        get { return m_origin; }
    }
}

public class PlayerController : LevelObjectСontroller
{
    enum State
    {
        Small = 0,
        Big,
        Super
    }

    [SerializeField]
    Rect m_BigColliderRect;

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

    [SerializeField]
    private PlayerShotInfo m_shot;

    State m_state;

    /* User move input */
    Vector2 m_moveInput;

    bool m_jumping;
    bool m_invincible;

    RuntimeAnimatorController m_initialAnimatorController;
    Rect m_initialColliderRect;

    #region Lifecycle

    protected override void OnAwake()
    {
        base.OnAwake();

        assert.IsNotNull(m_BigAnimatorController);

        m_initialAnimatorController = animator.runtimeAnimatorController;
        m_initialColliderRect = colliderRect;
        m_state = State.Small;
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();

        flipX = false;
        m_moveInput = Vector2.zero;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (dead) return;

        m_moveInput.x = Input.GetAxisRaw("Horizontal");
        m_moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("Jump") && grounded && !m_jumping)
        {
            StartJump(m_JumpHighSpeed);
        }

        if (Input.GetButtonDown("Shoot"))
        {
            Shot();
        }
    }

    #endregion

    #region Inheritance

    protected override void UpdateVelocity(float deltaTime)
    {
        base.UpdateVelocity(deltaTime);

        float vx = m_Velocity.x;
        float moveX = m_moveInput.x;
        
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
                vx = Mathf.Min(vx, CVars.g_playerWalkSpeed.FloatValue);
            }
            else if (vx < 0)
            {
                vx = Mathf.Max(vx, -CVars.g_playerWalkSpeed.FloatValue);
            }
        }
        
        if (moveX >  Mathf.Epsilon && direction == DIR_LEFT ||
            moveX < -Mathf.Epsilon && direction == DIR_RIGHT)
        {
            Flip();
        }
        
        m_Velocity.x = vx;

        if (!dead && grounded)
        {
            animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
            animator.SetBool("Stop", m_moveInput.x > Mathf.Epsilon && m_Velocity.x < 0 || m_moveInput.x < -Mathf.Epsilon && m_Velocity.x > 0);
        }
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

    protected override void OnStartFalling()
    {
        if (!m_jumping)
        {
            StartFall();
        }
    }

    protected override void OnStopFalling()
    {
        if (m_jumping)
        {
            EndJump();
        }
        else
        {
            EndFall();
        }
    }

    protected override void OnObstacle(Cell cell)
    {
        m_Velocity.x = 0f;
    }

    protected override void OnJumpHitCell(Cell cell)
    {
        if (!cell.jumping)
        {
            cell.Hit(this);
        }
    }

    protected override void OnDie()
    {
        m_moveInput = Vector2.zero;
        StartCoroutine(DieCoroutine());
    }

    #endregion

    #region State

    public void AdvanceState()
    {
        if (m_state < State.Super)
        {
            ChangeState(m_state + 1);
        }
    }

    internal void OnStartChangeStateAnimation()
    {
        Time.timeScale = 0; // stop everything when animation is played
    }

    internal void OnEndChangeStateAnimation()
    {
        Time.timeScale = 1; // resume everything

        if (m_jumping)
        {
            animator.SetBool("Jump", true);
        }
    }

    void ChangeState(State state)
    {
        switch (state)
        {
            case State.Small:
                animator.runtimeAnimatorController = m_initialAnimatorController;
                colliderRect = m_initialColliderRect;
                StartInvincibility();
                break;
            case State.Big:
            case State.Super:
                animator.runtimeAnimatorController = m_BigAnimatorController;
                colliderRect = m_BigColliderRect;
                break;
        }

        m_state = state;
        animator.SetBool("Jump", false);
        animator.SetTrigger("ChangeState");
    }

    void StartInvincibility()
    {
        assert.IsFalse(m_invincible);
        m_invincible = true;

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        assert.IsTrue(m_invincible);

        SpriteRenderer renderer = GetRequiredComponent<SpriteRenderer>();
        Color currentColor = renderer.color;
        Color clearColor = Color.clear;

        for (int i = 0; i < 203; ++i) // magic number of frames
        {
            renderer.color = i % 2 == 0 ? clearColor : currentColor;
            yield return null;
        }

        renderer.color = currentColor;
        m_invincible = false;
    }
    
    #endregion

    #region Jump

    void StartJump(float velocity)
    {
        if (!grounded)
        {
            EndFall();
        }

        m_jumping = true;
        m_Velocity.y = velocity;

        assert.IsTrue(animator.enabled);
        animator.SetBool("Jump", true);
    }

    void EndJump()
    {
        m_jumping = false;
        assert.IsTrue(animator.enabled);
        animator.SetBool("Jump", false);
    }

    void StartFall()
    {
        assert.IsFalse(grounded);
        assert.IsFalse(m_jumping);
        animator.enabled = false;
    }

    void EndFall()
    {
        assert.IsFalse(m_jumping);
        animator.enabled = true;
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

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    #endregion

    #region Shot

    private void Shot()
    {
        var shotObject = Instantiate(m_shot.prefab) as FireBallController;
        shotObject.transform.parent = transform.parent;
        shotObject.transform.position = m_shot.origin.position;
        shotObject.Launch(direction);
    }

    #endregion

    #region Collisions

    protected override void OnCollision(LevelObjectСontroller other)
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

    void OnJumpOnObject(LevelObjectСontroller other)
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

    void OnCollideObject(LevelObjectСontroller other)
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

    protected override void OnDamage(LevelObjectСontroller attacker)
    {
        if (m_state == State.Small)
        {
            Die();
        }
        else
        {
            ChangeState(State.Small);
        }
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
        m_Velocity.y = m_JumpSquashSpeed;
    }

    #endregion

    #region Properties

    public bool invincible
    {
        get { return m_invincible; }
    }

    public bool isSmall
    {
        get { return m_state == State.Small; }
    }

    #endregion
}