using UnityEngine;
using System.Collections;

namespace LunarCore
{
    public class DestroyDelayed : BaseBehaviour
    {
        [SerializeField]
        float m_Delay;

        protected override void OnStart()
        {
            Destroy(gameObject, m_Delay);
        }
    }
}
