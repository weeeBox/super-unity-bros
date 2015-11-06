using UnityEngine;
using System.Collections;

using LunarCore;

public interface IPowerup
{
}

public class Cell
{
    public readonly int i;
    public readonly int j;
    public readonly float x;
    public readonly float y;

    private readonly Map m_Map;

    public Cell(Map map, int i, int j)
    {
        m_Map = map;

        this.i = i;
        this.j = j;
        this.x = (0.5f + j) * Constants.CELL_WIDTH;
        this.y = (0.5f + i) * Constants.CELL_HEIGHT;
    }

    public virtual void Hit()
    {
    }

    protected Map map
    {
        get { return m_Map; }
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
}

class BrickCell : Cell
{
    public BrickCell(Map map, int i, int j)
        : base(map, i, j)
    {
    }

    public override void Hit()
    {
        map.Jump(i, j, OnHitFinished);
    }

    protected virtual void OnHitFinished()
    {
    }
}

class CoinsCell : BrickCell
{
    int m_Coins;

    public CoinsCell(Map map, int i, int j, int coins)
        : base(map, i, j)
    {
        assert.IsTrue(coins > 0);
        m_Coins = coins;
    }

    public override void Hit()
    {
        assert.IsTrue(m_Coins > 0);

        base.Hit();
        --m_Coins;

        GameObject coinObject = GameManager.CreateJumpingCoin();
        coinObject.transform.parent = map.transform;
        coinObject.transform.localPosition = new Vector3(x, y + 0.4f * 21, 0); // FIXME: remove magic number

        if (m_Coins == 0)
        {
            map.SetCell(i, j, Map.CELL_BLANK);
        }
    }
}

class PowerCell : BrickCell
{
    IPowerup m_Powerup;

    public PowerCell(Map map, int i, int j, IPowerup powerup)
        : base(map, i, j)
    {
        assert.IsNotNull(powerup);
        m_Powerup = powerup;
    }

    protected override void OnHitFinished()
    {
        // TODO: 
    }
}