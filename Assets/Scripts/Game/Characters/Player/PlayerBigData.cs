using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu]public class PlayerBigData : LevelObjectData
{
    [SerializeField]
    Rect m_duckColliderRect;

    [SerializeField]
    Rect m_duckHitRect;

    public Rect duckDolliderRect
    {
        get { return m_duckColliderRect; }
    }

    public Rect duckHitRect
    {
        get { return m_duckHitRect; }
    }
}
