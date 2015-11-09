using UnityEngine;
using System.Collections;

public class GoombaController : EnemyController
{
    bool m_Crushed;

    public override void OnPlayerJump(MarioController player)
    {
        if (!m_Crushed)
        {
            base.OnPlayerJump(player);
            Crush();
        }
    }

    public override void OnPlayerCollision(MarioController player)
    {
        if (!m_Crushed)
        {
            base.OnPlayerCollision(player);
        }
    }

    void Crush()
    {
        assert.IsFalse(m_Crushed);
        m_Crushed = true;
        m_Velocity = Vector2.zero;

        animator.SetBool("Crushed", true);
    }
}
