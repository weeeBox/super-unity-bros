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
            // TODO: start "unlock" timer
            m_LockedInShell = true;
            animator.SetBool("Shell", true);

            player.JumpOnEnemy(this);
        }
    }
}
