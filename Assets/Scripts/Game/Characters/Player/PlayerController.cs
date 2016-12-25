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
    PlayerBigData m_bigData;

    [SerializeField]
    private float m_JumpHighSpeed = 120.0f;

    [SerializeField]
    private float m_JumpSquashSpeed = 70.0f;

    [SerializeField]
    private RuntimeAnimatorController m_BigAnimatorController;

    [SerializeField]
    private PlayerShotInfo m_shot;

    State m_state;

    /* User move input */
    Vector2 m_moveInput;

    bool m_running;
    bool m_jumping;
    bool m_invincible;
    bool m_ducking;
    bool m_longJump;

    RuntimeAnimatorController m_initialAnimatorController;

    #region Lifecycle

    protected override void OnAwake()
    {
        base.OnAwake();

        assert.IsNotNull(m_BigAnimatorController);

        m_initialAnimatorController = animator.runtimeAnimatorController;
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
        m_running = Input.GetButton("Shoot");

        if (m_state != State.Small)
        {
            bool wasDucking = m_ducking;
            m_ducking = m_moveInput.y < -0.01f;
            if (m_ducking && !wasDucking && grounded)
            {
                SetDucking(true);
            }
            else if (wasDucking && !m_ducking)
            {
                SetDucking(false);
            }
        }

        if (Input.GetButtonDown("Jump") && grounded && !m_jumping)
        {
            StartJump(CVars.g_playerJumpSpeed.FloatValue);
            m_longJump = true;
        }

        // stop "long" jump if player stopped jumping or started falling or released "Jump" button
        if (m_longJump && (!m_jumping || m_Velocity.y < 0.001f || !Input.GetButton("Jump")))
        {
            m_longJump = false;
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
        if (m_longJump)
        {
            m_Velocity.y += CVars.g_playerGravityLongJump.FloatValue * deltaTime;
        }
        else
        {   
            m_Velocity.y += CVars.g_playerGravity.FloatValue * deltaTime;
        }
        m_Velocity.y = Mathf.Clamp(m_Velocity.y, -CVars.g_playerJumpSpeed.FloatValue, CVars.g_playerJumpSpeed.FloatValue);

        float vx = m_Velocity.x;
        float moveX = m_moveInput.x;
        
        if (Mathf.Approximately(moveX, 0.0f))
        {
            vx += -direction * CVars.g_playerWalkAcc.FloatValue * deltaTime;
            vx = direction > 0 ? Mathf.Max(vx, 0) : Mathf.Min(vx, 0);
        }
        else
        {
            bool breaking = moveX > Mathf.Epsilon && vx < -Mathf.Epsilon ||
                            moveX < -Mathf.Epsilon && vx > Mathf.Epsilon;
            float maxVelocity = m_running ? CVars.g_playerRunSpeed.FloatValue : CVars.g_playerWalkSpeed.FloatValue;
            float acceleration;
            if (m_running)
            {
                acceleration = breaking ? CVars.g_playerRunBreakAcc.FloatValue : CVars.g_playerRunAcc.FloatValue;
            }
            else
            {
                acceleration = breaking ? CVars.g_playerWalkBreakAcc.FloatValue : CVars.g_playerWalkAcc.FloatValue;
            }

            vx += moveX * acceleration * deltaTime;
            vx = Mathf.Clamp(vx, -maxVelocity, maxVelocity);
        }
        m_Velocity.x = vx;
        
        if (moveX >  Mathf.Epsilon && direction == DIR_LEFT ||
            moveX < -Mathf.Epsilon && direction == DIR_RIGHT)
        {
            Flip();
        }

        if (!dead && grounded)
        {
            animator.speed = m_running ? Mathf.Clamp(Mathf.Abs(m_Velocity.x) / CVars.g_playerWalkSpeed.FloatValue, 0.5f, 2.0f) : 1;
            animator.SetFloat("Speed", Mathf.Abs(m_Velocity.x));
            animator.SetBool("Stop", m_moveInput.x > Mathf.Epsilon && m_Velocity.x < 0 || m_moveInput.x < -Mathf.Epsilon && m_Velocity.x > 0);
        }
        else
        {
            animator.speed = 1;
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
        animator.SetBool("Jump", false);

        switch (state)
        {
            case State.Small:
                m_ducking = false;
                animator.runtimeAnimatorController = m_initialAnimatorController;
                colliderRect = data.colliderRect;
                hitRect = data.hitRect;
                animator.SetTrigger("Shrink");
                StartInvincibility();
                break;
            case State.Big:
            case State.Super:
                animator.runtimeAnimatorController = m_BigAnimatorController;
                colliderRect = m_bigData.colliderRect;
                hitRect = m_bigData.hitRect;
                animator.SetTrigger("Grow");
                break;
        }

        m_state = state;
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
        if (!m_ducking)
        {
            animator.SetBool("Jump", true);
        }
    }

    void EndJump()
    {
        m_jumping = false;
        assert.IsTrue(animator.enabled);
        animator.SetBool("Jump", false);

        if (m_ducking)
        {
            SetDucking(true);
        }
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

        if (m_ducking)
        {
            SetDucking(true);
        }
    }

    #endregion

    #region Death
    
    IEnumerator DieCoroutine()
    {
        animator.SetBool("Stop", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Dead", true);
        animator.SetBool("Duck", false);

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
//        var shotObject = Instantiate(m_shot.prefab) as FireBallController;
//        shotObject.transform.parent = transform.parent;
//        shotObject.transform.position = m_shot.origin.position;
//        shotObject.Launch(direction);
    }

    #endregion

    #region Duck

    private void SetDucking(bool ducking)
    {
        animator.SetBool("Duck", ducking);
        if (m_jumping)
        {
            animator.SetBool("Jump", true);
        }
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