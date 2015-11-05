using UnityEngine;
using System.Collections;

public class EnemyController : EntityController
{
    protected override void OnStart()
    {
        base.OnStart();

        m_Velocity.x = m_Direction * m_WalkSpeed;
    }

    protected override void UpdateVelocity(float deltaTime)
    {
        base.UpdateVelocity(deltaTime);
    }

    protected override void OnObstacle()
    {
        Flip();
        m_Velocity.x = -m_Velocity.x;
    }
}
