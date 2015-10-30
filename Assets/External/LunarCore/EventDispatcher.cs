using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LunarCore
{
    public delegate void EventListener(Event evt);

    public class Event : IObjectPoolEntry
    {
        #region Object Pool

        public void Recycle()
        {
            name = null;
            sender = null;
            data = null;
        }

        #endregion

        #region Properties

        public string name { get; internal set; }
        public object sender { get; internal set; }
        public object data { get; internal set; }

        #endregion
    }

    public class EventDispatcher
    {
        public static ObjectPool<Event> eventPool = new ObjectPool<Event>();

        private IDictionary<string, EventListenerList> listenersLookup;

        public void DispatchEvent(object sender, string name, object data)
        {
            assert.IsNotNull(sender);
            assert.IsNotNull(name);

            EventListenerList list = FindEventListenerList(name);
            if (list != null)
            {
                Event evt = eventPool.Take();
                if (evt == null)
                {
                    evt = new Event();
                }

                evt.sender = sender;
                evt.name = name;
                evt.data = data;

                list.Dispatch(evt);

                eventPool.Put(evt);
            }
        }

        public void AddListener(string name, EventListener listener)
        {
            assert.IsNotNull(name);
            assert.IsNotNull(listener);

            if (listenersLookup == null)
            {
                listenersLookup = new Dictionary<string, EventListenerList>();
            }

            EventListenerList list;
            if (!listenersLookup.TryGetValue(name, out list))
            {
                list = new EventListenerList();
                listenersLookup[name] = list;
            }

            list.Add(listener);
        }

        EventListenerList FindEventListenerList(string name)
        {
            EventListenerList list;
            if (listenersLookup != null && listenersLookup.TryGetValue(name, out list))
            {
                return list;
            }
            return null;
        }
    }

    class EventListenerList
    {
        private List<EventListener> listeners;

        internal void Dispatch(Event evt)
        {
            assert.IsTrue(listeners != null && listeners.Count > 0);
            for (int i = 0; i < listeners.Count; ++i)
            {
                listeners[i](evt);
            }
        }

        public void Add(EventListener listener)
        {
            assert.IsNotNull(listener);
            if (listeners == null)
            {
                listeners = new List<EventListener>(2);
            }

            assert.NotContains(listener, listeners);
            listeners.Add(listener);
        }
    }
}
