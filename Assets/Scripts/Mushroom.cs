using UnityEngine;
using System.Collections;

using LunarCore;

public class Mushroom : BaseBehaviour2D
{
    private Rigidbody2D m_RigidBody;

    protected override void OnStart()
    {
        m_RigidBody = GetRequiredComponent<Rigidbody2D>();
        m_RigidBody.isKinematic = true;

        StartCoroutine(Grow());
    }
        
    IEnumerator Grow()
    {
        yield return new WaitForSeconds(0.05f);

        float targetX = position2D.x;
        float targetY = position2D.y + 6.4f;
        position2D = new Vector2(position2D.x, position2D.y + 0.4f * 4);

        while (position2D.y < targetY)
        {
            yield return new WaitForSeconds(0.067f);
            position2D = new Vector2(targetX, position2D.y + 0.4f);
        }

        position2D = new Vector2(targetX, targetY);

        GrowFinished();
    }

    void GrowFinished()
    {
        m_RigidBody.isKinematic = false;
        m_RigidBody.velocity = new Vector2(36, 0);
    }
}
