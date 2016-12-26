using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarCore;

public delegate void MessageCenterDelegate<T>(T target) where T : class;

public static class MessageCenter
{
    public static void Send<T>(MessageCenterDelegate<T> callback) where T : class
    {
        foreach (var obj in GameObject.FindObjectsOfType<MonoBehaviour>())
        {
            T target = obj as T;
            if (target != null)
            {
                callback(target);
            }
        }
    }
}
