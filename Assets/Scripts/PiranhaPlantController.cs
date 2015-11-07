using UnityEngine;
using System.Collections;

using LunarCore;

public class PiranhaPlantController : BaseBehaviour2D
{
    [SerializeField]
    float m_Speed = 12f;

    [SerializeField]
    float m_IdleTime = 1f;

    [SerializeField]
    float m_HiddenTime = 1.25f;

    float m_Height;

    protected override void OnStart()
    {
        SpriteRenderer spriteRenderer = GetRequiredComponent<SpriteRenderer>();
        m_Height = spriteRenderer.sprite.bounds.size.y;

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        Vector3 idlePos = transform.localPosition;
        Vector3 hiddenPos = idlePos - new Vector3(0, m_Height, 0);

        while (true)
        {
            // Hide
            transform.localPosition = hiddenPos;
            yield return new WaitForSeconds(m_HiddenTime);

            // Up
            while (posY < idlePos.y)
            {
                transform.Translate(0, m_Speed * Time.fixedDeltaTime);
                yield return null;
            }

            // Idle
            transform.localPosition = idlePos;
            yield return new WaitForSeconds(m_IdleTime);

            // Down
            while (posY > hiddenPos.y)
            {
                transform.Translate(0, -m_Speed * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }
}
