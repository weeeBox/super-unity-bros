using UnityEngine;
using System.Collections;

using LunarCore;

public class BrickFragment : BaseBehaviour2D
{
    [SerializeField]
    Vector2 m_Speed;

    [SerializeField]
    float m_FlipDelay = 0.15f;

    Vector3 m_Velocity;
    float m_FlipElasped;

    protected override void OnStart()
    {
        m_Velocity = m_Speed;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        m_Velocity.y += Constants.GRAVITY * deltaTime;
        transform.Translate(m_Velocity * deltaTime);

        m_FlipElasped += deltaTime;
        if (m_FlipElasped >= m_FlipDelay)
        {
            m_FlipElasped = 0f;
            flipX = !flipX;
        }
    }
}
