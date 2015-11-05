using UnityEngine;
using System.Collections;

public class EntityController : MovingObject
{
    [SerializeField]
    protected float m_WalkSpeed;

    protected Animator m_Animator;
    
    protected override void OnAwake()
    {
        m_Animator = GetRequiredComponent<Animator>();
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        base.OnFixedUpdate(deltaTime);

        UpdateAnimation(deltaTime);
    }

    protected virtual void UpdateAnimation(float deltaTime)
    {
    }
}
