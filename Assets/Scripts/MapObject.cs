using UnityEngine;
using System.Collections;

using LunarCore;

public abstract class MapObject : BaseBehaviour2D
{
    bool m_Sleeping;

    void Start()
    {
        sleeping = true;
        OnStart();
    }

    void FixedUpdate()
    {
        if (m_Sleeping)
        {
            if (left > camera.right) return; // not visible yet
            
            sleeping = false;
            OnBecomeVisible();
        }

        OnFixedUpdate(Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        MapObject obj = other.GetComponent<MapObject>();
        if (obj != null && !obj.sleeping)
        {
            OnCollision(obj);
        }
    }

    #region Callbacks
    
    protected virtual void OnBecomeVisible()
    {
    }
    
    protected virtual void OnBecomeInvisible()
    {
    }

    protected virtual void OnCollision(MapObject other)
    {
    }

    #endregion

    #region Properties

    public bool sleeping
    {
        get { return m_Sleeping; }
        set { m_Sleeping = value; }
    }

    protected Map map
    {
        get { return GameManager.map; }
    }
    
    protected GameCamera camera
    {
        get { return GameManager.camera; }
    }

    public abstract float left
    {
        get; set;
    }
    
    public abstract float right
    {
        get; set;
    }
    
    public abstract float top
    {
        get; set;
    }
    
    public abstract float bottom
    {
        get; set;
    }

    #endregion
}
