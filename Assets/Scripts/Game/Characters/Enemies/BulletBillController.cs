using UnityEngine;
using System.Collections;

using LunarCore;

public class BulletBillController : BaseBehaviour2D
{
    [SerializeField]
    float m_Speed = 36f;

    int m_Direction;

    protected override void OnFixedUpdate(float deltaTime)
    {
        transform.Translate(m_Direction * m_Speed * deltaTime, 0f);
    }

    #region Properties

    public int direction
    {
        get { return m_Direction; }
        set
        {
            assert.IsTrue(value == 1 || value == -1);

            m_Direction = value;
            flipX = value == -1;
        }
    }

    #endregion
}
