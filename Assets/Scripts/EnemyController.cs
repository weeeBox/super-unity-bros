using UnityEngine;
using System.Collections;

public class EnemyController : LevelObject
{
    [SerializeField]
    float m_DamageJumpSpeed = 50;

    [SerializeField]
    float m_DamageImpactSpeed = 24;

    /// <summary>
    /// Called when player jumped on the enemy.
    /// </summary>
    public virtual void OnPlayerJump(MarioController player)
    {
        player.JumpOnEnemy(this);
    }

    /// <summary>
    /// Called when collided with player (not a jump)
    /// </summary>
    public virtual void OnPlayerCollision(MarioController player)
    {
        player.TakeDamage(this);
    }

    protected override void OnStart()
    {
        base.OnStart();

        m_Velocity.x = direction * walkSpeed;
        FlipHorMovement();
    }

    protected override void OnObstacle(Cell cell)
    {
        FlipHorMovement();
    }

    protected override void OnCellJumped(Cell cell)
    {
        assert.IsTrue(cell.jumping);
        TakeDamage(cell.jumpAttacker);
    }

    #region Collisions

    protected override void OnCollision(LevelObject other)
    {
        EnemyController enemy = other as EnemyController;
        if (enemy != null)
        {
            OnCollision(enemy);
        }
    }

    protected virtual void OnCollision(EnemyController enemy)
    {
        FlipHorMovement();
    }

    #endregion

    #region Damage
    
    protected override void OnDamage(LevelObject attacker)
    {
        collisionsEnabled = false;

        flipY = true;
        m_Velocity.x = attacker.transform.position.x < transform.position.x ? m_DamageImpactSpeed : -m_DamageImpactSpeed;
        m_Velocity.y = m_DamageJumpSpeed;

        SetComponentEnabled<Animator>(false); // don't play any animation
    }
    
    #endregion
}
