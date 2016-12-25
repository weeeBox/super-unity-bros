using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionTileType
{
    Coins,
    Mushroom
}

[CreateAssetMenu]
public class QuestionTile : HittableTile
{
    [SerializeField]
    private QuestionTileType m_type = QuestionTileType.Coins;

    [SerializeField]
    private int m_coinCount = 1;

    public QuestionTileType type
    {
        get { return m_type; }
        set { m_type = value; }
    }

    public int cointCount
    {
        get { return m_coinCount; }
        set { m_coinCount = value; }
    }
}
