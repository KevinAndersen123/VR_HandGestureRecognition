using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    bool holding_axe = true;
    public void Spawn(int index)
    {
        if(!holding_axe)
            Instantiate(prefabs[index], transform.position, transform.rotation);
    }

    public void StopSpawn()
    {
        Debug.Log("Stop Spawning");
    }
}
