using UnityEngine;
using System.Collections;

public class KoopaTroopaController : EnemyController
{
    [SerializeField]
    private float m_ShellSpeed = 72;

    private bool m_LockedInShell;

    public override void OnJumped(MarioController player)
    {
        if (m_LockedInShell)
        {
            if (Mathf.Approximately(m_Velocity.x, 0))
            {
                if (player.posX > posX)
                {
                    m_Velocity.x = -m_ShellSpeed;
                }
                else
                {
                    m_Velocity.x = m_ShellSpeed;
                }
            }
            else
            {
                m_Velocity.x = 0;
                player.JumpOnEnemy(this);
            }
        }
        else
        {
            // TODO: start "unlock" timer
            m_Velocity.x = 0;
            m_LockedInShell = true;
            animator.SetBool("Shell", true);

            player.JumpOnEnemy(this);
        }
    }

    public override void OnHitByPlayer(MarioController player)
    {
        if (m_LockedInShell && Mathf.Approximately(m_Velocity.x, 0f))
        {
            if (player.posX > posX)
            {
                m_Velocity.x = -m_ShellSpeed;
            }
            else
            {
                m_Velocity.x = m_ShellSpeed;
            }
        }
        else
        {
            base.OnHitByPlayer(player);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;
        
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (m_LockedInShell && !Mathf.Approximately(m_Velocity.x, 0f))
            {
                enemy.Die(true);
            }
            else
            {
                Flip();
                m_Velocity.x = -m_Velocity.x;
            }
        }
    }
}
