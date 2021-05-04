using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    //checks if the weapon hits the player
    //deals 10 damage each time it hits
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(10);
        }
    }
}
