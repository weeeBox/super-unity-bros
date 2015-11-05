using UnityEngine;
using System.Collections;

public class EntityController : MovingObject
{
    [SerializeField]
    float m_WalkSpeed;

    Animator m_Animator;

    #region Lifecycle

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Animator = GetRequiredComponent<Animator>();
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);

        UpdateAnimation(deltaTime);
        CheckFallingOffMap();
    }

    #endregion

    #region Inheritance

    protected virtual void UpdateAnimation(float deltaTime)
    {
    }

    #endregion

    #region Callbacks

    protected override void OnDie(bool animated)
    {
        base.OnDie(animated);

        animator.SetBool("Dead", true);
    }

    #endregion

    #region Death

    private void CheckFallingOffMap()
    {
        if (!dead && top < -3.2f) // FIXME: remove magic number
        {
            Die(false);
        }
    }

    #endregion

    #region Properties

    protected Animator animator
    {
        get { return m_Animator; }
    }

    protected float walkSpeed
    {
        get { return m_WalkSpeed; }
    }

    #endregion
}
