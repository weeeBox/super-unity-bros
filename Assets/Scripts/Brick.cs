using UnityEngine;
using System.Collections;

using LunarCore;

public class Brick : BaseBehaviour2D
{
    private static readonly int[] JUMP_OFFSETS = {
        1, 3, 5, 6, 6, 7, 7, 7, 6, 5, 4, 3, 1, -1
    };

    [SerializeField]
    private Mushroom m_Mushroom;

    protected override void OnStart()
    {
        Assert.IsNotNull(m_Mushroom);
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.position.y < transform.position.y)
        {
            Hit();
        }
    }

    void Hit()
    {
        Jump();
    }

    void Jump()
    {
        StartCoroutine(JumpRoutine());
    }

    void JumpFinished()
    {
        Mushroom mushroom = Instantiate(m_Mushroom) as Mushroom;
        mushroom.transform.parent = transform;
        mushroom.transform.localPosition = Vector3.zero;
    }

    IEnumerator JumpRoutine()
    {
        float x = position2D.x;
        float y = position2D.y;
        for (int i = 0; i < JUMP_OFFSETS.Length; ++i)
        {
            position2D = new Vector2(x, y + 0.4f * JUMP_OFFSETS[i]);
            yield return null;
        }

        position2D = new Vector2(x, y);

        JumpFinished();
    }
}
