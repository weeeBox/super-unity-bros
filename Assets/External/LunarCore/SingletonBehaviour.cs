using UnityEngine;
using System.Collections;

namespace LunarCore
{
    public class SingletonBehaviour<T> : BaseBehaviour where T : BaseBehaviour
    {
        private static BaseBehaviour instanceRef;

        #region Life cycle

        void Awake()
        {
            if (instanceRef == null)
            {
                instanceRef = this;
                DontDestroyOnLoad(gameObject);
                OnAwake();
            }
            else if (instanceRef != this)
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            if (instance == this)
            {
                OnStart();
            }
        }

        #endregion

        #region Properties

        public static T instance
        {
            get { return (T) instanceRef; }
        }

        #endregion
    }
}
