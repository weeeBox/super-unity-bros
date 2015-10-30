using UnityEngine;
using System.Collections;

using LunarCore;

public class GameManager : SingletonBehaviour<GameManager>
{
    private Map m_Map;

    protected override void OnStart()
    {
        m_Map = FindObjectOfType<Map>();
        assert.IsNotNull(m_Map);
    }

    public static Map map
    {
        get
        {
            assert.IsNotNull(instance);
            return instance.m_Map;
        }
    }
}
