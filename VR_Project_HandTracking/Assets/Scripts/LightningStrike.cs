using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    //render the hand pointer raycast
    public LineRenderer m_lightningStrikeLine;

    //info about the line renderer
    public float m_lineWidth = 0.1f;
    public float m_lineMaxLength = 1f;

    //bool to determine if line is eneabled or not
    public bool m_lineToggle;

    public bool isStriking = false;
    //bool for if we hit enemy with raycast
    public bool m_enemyHit = false;

    //Enemy
    private GameObject m_enemy = null;

    void Start()
    {
        Vector3[] startLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        m_lightningStrikeLine.SetPositions(startLinePositions);
        m_lightningStrikeLine.enabled = false;
    }

    void Update()
    {
        isStriking = FindObjectOfType<Player>().m_isLightningStrike;

        if(isStriking)
        {
            m_lineToggle = true;
            m_lightningStrikeLine.enabled = true;
        }
        else
        {
            m_lightningStrikeLine.enabled = false;
            m_lineToggle = false;
            //make sure we cant register a hit on enemy when the line renderer is turned off
            m_enemyHit = false;
        }

        if(m_lineToggle)
        {
            Lightning(transform.position, transform.forward, m_lineMaxLength);
        }
    }

    private void Lightning(Vector3 t_targetPos, Vector3 t_direction, float t_length)
    {
        //setup raycast hit
        RaycastHit hit;

        // Setup raycast
        Ray m_lightningOut = new Ray(t_targetPos, t_direction);

        //declares an end pos variable for the line renderer
        Vector3 endPos = t_targetPos + (t_length * t_direction);

        //run raycast
        if(Physics.Raycast(m_lightningOut,out hit))
        {
            //update line render with new end pos
            endPos = hit.point;
            
            //set enemy gameobj to the gameobj that the raycast hit
            if(hit.collider.gameObject.tag == "Enemy")
            {
                m_enemy = hit.collider.gameObject;
            }
            //if enemy has the explode script, do something
            if (m_enemy != null)//m_enemy.GetComponent<Enemy>())
            {
                m_enemyHit = true;
                //m_enemy.GetComponent<Enemy>().TakeDamage();
            }
            else
            {
                m_enemyHit = false;
            }
        }
        //if raycast stops, set enemy hit to false
        else if(m_enemyHit)
        {
            m_enemyHit = false;
        }
        //update line renderer declared at top
        m_lightningStrikeLine.SetPosition(0, t_targetPos);
        m_lightningStrikeLine.SetPosition(1, endPos);
    }
}
