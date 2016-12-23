using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class LevelObjectData : ScriptableObject
{
    [SerializeField]
    Rect m_colliderRect; // map collision

    [SerializeField]
    Rect m_hitRect; // hit collision

    public Rect colliderRect
    {
        get { return m_colliderRect; }
    }

    public Rect hitRect
    {
        get { return m_hitRect; }
    }
}
