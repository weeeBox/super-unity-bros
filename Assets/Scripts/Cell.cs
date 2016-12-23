using UnityEngine;
using System.Collections;

using LunarCore;

public enum PowerupType
{
    Mushroom,
    Start,
    Life
}

public class Cell
{
    public readonly int i;
    public readonly int j;
    public readonly float x;
    public readonly float y;

    readonly Map m_Map;
    LevelObjectСontroller m_JumpAttacker;

    public Cell(Map map, int i, int j)
    {
        m_Map = map;

        this.i = i;
        this.j = j;
        this.x = (0.5f + j) * Constants.CELL_WIDTH;
        this.y = (0.5f + i) * Constants.CELL_HEIGHT;
    }

    public virtual void Hit(PlayerController attacker)
    {
    }

    #region Properties

    protected Map map
    {
        get { return m_Map; }
    }

    public bool jumping
    {
        get { return m_JumpAttacker != null; }
    }

    public LevelObjectСontroller jumpAttacker
    {
        get { return m_JumpAttacker; }
        protected set { m_JumpAttacker = value; }
    }

    public Vector3 position
    {
        get { return new Vector3(x, y, 0); }
    }

    public float top
    {
        get { return y + 0.5f * Constants.CELL_HEIGHT; }
    }

    public float bottom
    {
        get { return y - 0.5f * Constants.CELL_HEIGHT; }
    }

    public float left
    {
        get { return x - 0.5f * Constants.CELL_WIDTH; }
    }

    public float right
    {
        get { return x + 0.5f * Constants.CELL_WIDTH; }
    }

    #endregion
}

class HittableCell : Cell
{
    protected bool m_blank;

    public HittableCell(Map map, int i, int j)
        : base(map, i, j)
    {
    }

    public override void Hit(PlayerController attacker)
    {
        if (!m_blank)
        {
            OnHit(attacker);
        }
    }

    protected virtual void OnHit(PlayerController attacker)
    {
        jumpAttacker = attacker;
        if (attacker.isSmall)
        {
            OnHitSmall(attacker);
        }
        else
        {
            OnHitBig(attacker);
        }
    }

    protected virtual void OnHitSmall(PlayerController attacker)
    {
        Jump();
    }

    protected virtual void OnHitBig(PlayerController attacker)
    {
        Jump();
    }

    protected virtual void Jump()
    {
        map.Jump(i, j, JumpFinished);
    }

    void JumpFinished()
    {
        jumpAttacker = null;
        OnHitFinished();
    }

    protected void SetBlank()
    {
        assert.IsFalse(m_blank);

        m_blank = true;
        map.SetTile(i, j, Map.CELL_BLANK);
    }

    protected virtual void OnHitFinished()
    {
    }
}

class BrickCell : HittableCell
{
    public BrickCell(Map map, int i, int j)
        : base(map, i, j)
    {
    }

    protected override void OnHitBig(PlayerController attacker)
    {
        map.InvokeLater(BreakTile); // we need to wait a frame in case if there's an enemy standing on the brick
    }

    void BreakTile()
    {
        map.RemoveTile(i, j);
        
        GameObject brokenBrick = GameManager.CreateBrokenBrick();
        brokenBrick.transform.parent = map.transform;
        
        Vector3 pos = position;
        pos.y += 1.2f; // FIXME: remove magic
        brokenBrick.transform.localPosition = pos;

        jumpAttacker = null;
    }
}

class CoinsCell : HittableCell
{
    int m_Coins;

    public CoinsCell(Map map, int i, int j, int coins)
        : base(map, i, j)
    {
        assert.IsTrue(coins > 0);
        m_Coins = coins;
    }

    protected override void OnHit(PlayerController attacker)
    {
        assert.IsTrue(m_Coins > 0);

        base.OnHit(attacker);

        --m_Coins;

        GameObject coinObject = GameManager.CreateJumpingCoin();
        coinObject.transform.parent = map.transform;
        coinObject.transform.localPosition = new Vector3(x, y + 0.4f * 21, 0); // FIXME: remove magic number

        if (m_Coins == 0)
        {
            SetBlank();
        }
    }
}

class PowerCell : HittableCell
{
    PowerupType m_PowerupType;

    public PowerCell(Map map, int i, int j, PowerupType powerup)
        : base(map, i, j)
    {
        m_PowerupType = powerup;
    }

    protected override void OnHit(PlayerController attacker)
    {
        base.OnHit(attacker);
        SetBlank();
    }

    protected override void OnHitFinished()
    {
        GameObject powerup = GameManager.CreatePowerup(m_PowerupType);
        powerup.transform.parent = map.transform;
        powerup.transform.localPosition = position;
    }
}