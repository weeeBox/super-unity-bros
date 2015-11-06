using UnityEngine;
using System.Collections;

using LunarCore;

[System.Serializable]
public class Prefabs
{
    public GameObject coin;
    public GameObject mushroom;
}

public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField]
    private Prefabs m_Prefabs;

    private Map m_Map;

    protected override void OnStart()
    {
        m_Map = FindObjectOfType<Map>();
        assert.IsNotNull(m_Map);
    }

    public static GameObject CreateJumpingCoin()
    {
        assert.NotNull(instance);
        assert.NotNull(instance.m_Prefabs.coin);

        return GameObject.Instantiate(instance.m_Prefabs.coin) as GameObject;
    }

    public static GameObject CreatePowerup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Mushroom:
                return GameObject.Instantiate(instance.m_Prefabs.mushroom) as GameObject;

            default:
                assert.Fail("Not supported: " + type);
                break;
        }

        return null;
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
