using UnityEngine;
using UnityEditor;

using System.Collections;

namespace LunarCore
{
    public class BaseEditor<T> : Editor where T : Object
    {
        protected virtual void OnSceneGUI()
        {
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        protected new T target
        {
            get { return (T) base.target; }
        }
    }
}
