using UnityEngine;
using System.Collections;

using LunarCore;

public class FollowTarget : BaseBehaviour
{
    public Transform m_Target;
    private Vector3 m_Offset;

    protected override void OnStart()
    {
        m_Offset = transform.position - m_Target.position;
    }

    protected override void OnUpdate(float deltaTime)
    {
        transform.position = m_Target.position + m_Offset;
    }
}
