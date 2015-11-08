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

    protected override void OnStart()
    {
        Camera camera = GetRequiredComponent<Camera>();
        m_HalfHeight = camera.orthographicSize;
        m_HalfWidth = camera.aspect * m_HalfHeight;
    }

    protected override void OnFixedUpdate(float deltaTime)
    {
        float playerX = player.transform.position.x;
        Vector3 cameraPos = transform.position;

        float distance = cameraPos.x - playerX;
        if (distance < m_BorderLow)
        {
            cameraPos.x = playerX + m_BorderLow;
            transform.position = cameraPos;
        }
    }

    #region Properties

    protected MarioController player
    {
        get { return GameManager.player; }
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
