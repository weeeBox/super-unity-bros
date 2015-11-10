using UnityEngine;
using System.Collections;

using LunarCore;

[System.Serializable]
public class Prefabs
{
    public GameObject coin;
    public GameObject mushroom;
}

public class GameManager : BaseBehaviour
{
    static GameManager s_Instance;

    [SerializeField]
    Prefabs m_Prefabs;

    [SerializeField]
    MarioController m_Player;

    [SerializeField]
    GameCamera m_Camera;

    Map m_Map;

    protected override void OnAwake()
    {
        s_Instance = this;
    }

    protected override void OnStart()
    {
        assert.IsNotNull(m_Player);
        assert.IsNotNull(m_Camera);

        m_Map = FindObjectOfType<Map>();
        assert.IsNotNull(m_Map);
    }

    protected override void OnDestroyed()
    {
        s_Instance = null;
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

    #region Properties

    public static Map map
    {
        get { return existingInstance.m_Map; }
    }

    public static GameCamera camera
    {
        get { return existingInstance.m_Camera; }
    }

    public static MarioController player
    {
        get { return existingInstance.m_Player; }
    }

    public static GameManager existingInstance
    {
        get
        {
            assert.IsNotNull(instance);
            return instance;
        }
    }

    public static GameManager instance
    {
        get { return s_Instance; }
    }

    #endregion
}
