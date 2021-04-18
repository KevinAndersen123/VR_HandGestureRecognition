using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(10);
        }
    }
}
