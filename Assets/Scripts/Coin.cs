using UnityEngine;
using System.Collections;

using LunarCore;

public class Coin : BaseBehaviour
{
    [SerializeField]
    private float m_InitialSpeed = 114f;

    [SerializeField]
    private float m_Gravity = -427.5f;

    private Vector3 m_InitialPos;
    private Vector3 m_Velocity;

    protected override void OnStart()
    {
        m_InitialPos = transform.position;
        m_Velocity = new Vector3(0, m_InitialSpeed, 0);
    }
    
    protected override void OnFixedUpdate(float deltaTime)
    {
        Vector3 offset = m_Velocity * deltaTime;
        m_Velocity.y += m_Gravity * deltaTime;

        transform.Translate(offset);

        if (transform.position.y <= m_InitialPos.y)
        {
            Destroy(gameObject);
        }
    }
}
