using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlantCollider : MonoBehaviour
{
    [SerializeField]
    PiranhaPlantController m_controller;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == Tags.Player)
        {
            m_controller.canAttack = false;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == Tags.Player)
        {
            m_controller.canAttack = true;
        }
    }
}