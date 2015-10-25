using UnityEngine;
using System.Collections.Generic;

namespace LunarCore
{
    public interface IObjectPoolEntry
    {
        void Recycle();
    }

    public class ObjectPool<T> where T : IObjectPoolEntry
    {
        private IList<T> objects;

        public ObjectPool()
        {
            objects = new List<T>();
        }

        public T Take()
        {
            if (objects.Count > 0)
            {
                int lastIndex = objects.Count - 1;
                T obj = objects[lastIndex];
                objects.RemoveAt(lastIndex);
                return obj;
            }

            return default(T);
        }

        public void Put(T obj)
        {
            Assert.IsNotNull(obj);
            obj.Recycle();
            objects.Add(obj);
        }

        public void Clear()
        {
            objects.Clear();
        }
    }
}
