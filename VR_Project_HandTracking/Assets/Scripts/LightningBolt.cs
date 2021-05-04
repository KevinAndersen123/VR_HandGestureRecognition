using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    //speed
    public float m_speed = 10.0f;

    //fire rate 
    public float m_fireRate = 1.0f;

    // damage of lighting bolt
    public float m_damage = 20.0f;

    //how many seconds have passed before it destroys itself
    public float m_timeBeforeDestroyed = 5.0f;

    //to check if collided
    private bool m_collided = false;

    //rigidbody
    private Rigidbody m_rb = null;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Destroy(gameObject, m_timeBeforeDestroyed);
    }

    void FixedUpdate()
    {
        //move bolt
        if(m_speed != 0 && m_rb != null)
        {
            m_rb.position += (transform.right) * (m_speed * Time.deltaTime);
        }
    }

    //checks if collided with anything other than itself
    public void OnCollisionEnter(Collision t_col)
    {
        if(t_col.gameObject.tag != "Lightning" && !m_collided)
        {
            m_collided = true;

            m_speed = 0;
            Destroy(gameObject);
        }
    }
}
