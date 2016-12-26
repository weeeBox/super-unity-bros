using UnityEngine;
using System.Collections;

using LunarCore;

[RequireComponent(typeof(Camera))]
public class GameCamera : BaseBehaviour
{
    [SerializeField]
    float m_BorderLow = 16f;
    
    [SerializeField]
    float m_BorderHi = 9.6f;

    float m_HalfWidth;
    float m_HalfHeight;
    float m_MaxPos;

    protected override void OnStart()
    {
        Camera camera = GetRequiredComponent<Camera>();
        m_HalfHeight = camera.orthographicSize;
        m_HalfWidth = camera.aspect * m_HalfHeight;
        m_MaxPos = map.transform.position.x + map.width - m_HalfWidth;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        var player = GameManager.player;
        if (player != null)
        {
            float playerX = player.transform.position.x;
            Vector3 cameraPos = transform.position;

            float distance = cameraPos.x - playerX;
            if (distance < m_BorderLow)
            {
                cameraPos.x = playerX + m_BorderLow;
            }

            cameraPos.x = Mathf.Min(cameraPos.x, m_MaxPos);

            transform.position = cameraPos;
        }
    }

    #region Properties

    protected Map map
    {
        get { return GameManager.map; }
    }

    public float top
    {
        get { return transform.localPosition.y + m_HalfHeight; }
    }

    public float left
    {
        get { return transform.localPosition.x - m_HalfWidth; }
    }

    public float bottom
    {
        get { return transform.localPosition.y - m_HalfHeight; }
    }

    public float right
    {
        get { return transform.localPosition.x + m_HalfWidth; }
    }

    public float width
    {
        get { return 2 * m_HalfWidth; }
    }

    public float height
    {
        get { return 2 * m_HalfHeight; }
    }

    #endregion
}
