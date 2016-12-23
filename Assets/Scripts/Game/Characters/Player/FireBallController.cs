using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : LevelObjectСontroller
{
    private bool m_touchedGround;

    public void Launch(int dir)
    {
        m_Velocity.x = dir * walkSpeed;
        m_Velocity.y = -walkSpeed;
    }

    protected override void UpdateVelocity(float deltaTime)
    {
        if (m_touchedGround)
        {
            m_Velocity.y += -1100 * deltaTime;
        }
    }

    protected override void OnCollision(LevelObjectСontroller other)
    {
        var enemy = other as EnemyController;
        if (enemy != null)
        {
            enemy.OnPlayerShot();
            Destroy(gameObject);
        }
    }

    protected override void OnObstacle(Cell cell)
    {
        Destroy(gameObject);
    }

    protected override void OnStopFalling()
    {
        m_touchedGround = true;
        m_Velocity.y = walkSpeed;
    }
}
