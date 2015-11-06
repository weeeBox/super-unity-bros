using UnityEngine;
using System.Collections;

public class EnemyController : EntityController
{
    public virtual void OnJumped(MarioController player)
    {
        Die();
        player.JumpOnEnemy(this);
    }

    protected override void OnStart()
    {
        base.OnStart();
        m_Velocity.x = direction * walkSpeed;
    }

    protected override void OnObstacle(Cell cell)
    {
        Flip();
        m_Velocity.x = -m_Velocity.x;
    }
}
