using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    //transform of bullet starting point
    public Transform m_startPoint;

    //gameobj used for bullet
    public GameObject m_lightningStrikePrefab;
    public enum ShootMode
    { 
        Auto,
        Single
    }
    //type of method of shots 
    public ShootMode m_shotMode;

    //bool for single shot mode
    private bool m_hasShoot = false;

    //float used to calculate the time needed to fire a lightning bolt, related to firerate
    private float m_timeToFire = 0f;

    //method to add in Event of the gesture you want to make shoot
    public void OnShoot()
    {
        switch (m_shotMode)
        {
            case ShootMode.Auto:
                if(Time.time >= m_timeToFire)
                {
                    m_timeToFire = Time.time + 1.0f / m_lightningStrikePrefab.GetComponent<LightningBolt>().m_fireRate;
                    Shoot();
                }
                break;

            case ShootMode.Single:
                if(!m_hasShoot)
                {
                    m_hasShoot = true;
                    m_timeToFire = Time.time + 1.0f / m_lightningStrikePrefab.GetComponent<LightningBolt>().m_fireRate;
                    Shoot();
                }
                break;
        }
    } 
    private void Shoot()
    {
        //spawn LighningStrike
        GameObject strike = Instantiate(m_lightningStrikePrefab, m_startPoint.position, Quaternion.identity);
        strike.transform.Rotate(m_startPoint.rotation.eulerAngles);
    }
    //method to call when gesture is not recognised 
    public void StopShoot()
    {
        m_hasShoot = false;
    }
}
