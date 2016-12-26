using UnityEngine;
using System.Collections;

using LunarCore;

[System.Serializable]
public class Prefabs
{
    public GameObject coin;
    public GameObject mushroom;
    public GameObject flower;
    public GameObject brokenBrick;
}

public class GameManager : BaseBehaviour, IPlayerControllerDelegate
{
    static GameManager s_Instance;

    [SerializeField]
    Prefabs m_Prefabs;

    PlayerController m_Player;
    GameCamera m_Camera;
    Map m_Map;

    protected override void OnAwake()
    {
        s_Instance = this;
        m_Player = FindObjectOfType<PlayerController>();
        m_Camera = FindObjectOfType<GameCamera>();
        m_Map = FindObjectOfType<Map>();
    }

    protected override void OnDestroyed()
    {
        s_Instance = null;
    }

    #region IPlayerControllerDelegate implementation

    public void OnPlayerDied(PlayerController player)
    {
        m_Player = null;
    }

    #endregion

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
                return GameObject.Instantiate(player.isSmall ? instance.m_Prefabs.mushroom : instance.m_Prefabs.flower) as GameObject;

            default:
                assert.Fail("Not supported: " + type);
                break;
        }

        return null;
    }

    public static GameObject CreateBrokenBrick()
    {
        return GameObject.Instantiate(instance.m_Prefabs.brokenBrick) as GameObject;
    }

    #region Properties

    public static Map map
    {
        get { return existingInstance.m_Map; }
    }

    public static new GameCamera camera
    {
        get { return existingInstance.m_Camera; }
    }

    public static PlayerController player
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
