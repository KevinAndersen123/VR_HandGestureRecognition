using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int m_health = 100;

    public void TakeDamage(int t_val)
    {
        m_health -= t_val;
        if(m_health <= 0)
        {
            Debug.Log("HIT ENEMY!");
            Destroy(this.gameObject);
        }
    }
}
