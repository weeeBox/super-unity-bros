using UnityEngine;
using System.Collections;

using LunarCore;

public class PiranhaPlantController : EnemyController
{
    float m_height;

    protected override void OnEnabled()
    {
        base.OnEnabled();

        var spriteRenderer = GetRequiredComponent<SpriteRenderer>();
        m_height = spriteRenderer.sprite.bounds.size.y;
        canAttack = true;
        movementEnabled = false;
        mapCollisionsEnabled = false;
        killsOnTouch = true;
        flipsWhenKilled = false;

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        Vector3 idlePos = transform.localPosition;
        Vector3 hiddenPos = idlePos - new Vector3(0, m_height, 0);

        while (true)
        {
            // Hide
            transform.localPosition = hiddenPos;
            yield return new WaitForSeconds(CVars.g_piranhaPlantHiddenTime.FloatValue);

            if (canAttack)
            {
                // Up
                while (posY < idlePos.y)
                {
                    transform.Translate(0, CVars.g_piranhaPlantSpeed.FloatValue * Time.fixedDeltaTime);
                    yield return null;
                }

                // Idle
                transform.localPosition = idlePos;
                yield return new WaitForSeconds(CVars.g_piranhaPlantIdleTime.FloatValue);

                // Down
                while (posY > hiddenPos.y)
                {
                    transform.Translate(0, -CVars.g_piranhaPlantSpeed.FloatValue * Time.fixedDeltaTime);
                    yield return null;
                }
            }
        }
    }

    public bool canAttack
    {
        get;
        set;
    }
}
