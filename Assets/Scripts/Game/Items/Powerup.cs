using UnityEngine;
using System.Collections;

public abstract class Powerup : LevelObjectСontroller
{
    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(Grow());
    }

    protected override void OnCellJumped(Cell cell)
    {
        assert.IsTrue(cell.jumping);

        if (cell.x > posX && direction == DIR_RIGHT || cell.x < posX && direction == DIR_LEFT)
        {
            FlipHorMovement();
        }
        m_Velocity.y = 75f; // FIXME: magic number
    }

    public abstract void Apply(PlayerController player);

    IEnumerator Grow()
    {
        // FIXME: fix this whole part - it's just terrible!
        mapCollisionsEnabled = false;
        movementEnabled = false;

        yield return new WaitForSeconds(0.05f);
        
        float targetX = position2D.x;
        float targetY = position2D.y + 6.4f;
        position2D = new Vector2(position2D.x, position2D.y + 0.4f * 4);
        
        while (position2D.y < targetY)
        {
            yield return new WaitForSeconds(0.067f);
            position2D = new Vector2(targetX, position2D.y + 0.4f);
        }
        
        position2D = new Vector2(targetX, targetY);
        mapCollisionsEnabled = true;
        movementEnabled = true;
        
        GrowFinished();
    }
    
    void GrowFinished()
    {
        m_Velocity.x = walkSpeed;
    }
}
