using UnityEngine;
using System.Collections;

public class EnemyController : EntityController
{
    protected override void OnStart()
    {
        base.OnStart();

        m_Velocity.x = direction * walkSpeed;
    }

    protected override void OnObstacle()
    {
        Flip();
        m_Velocity.x = -m_Velocity.x;
    }
}
