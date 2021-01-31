using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> objects;

    public void Spawn(string objectName)
    {
        foreach(var obj in objects)
        {
            obj.SetActive(objectName == obj.name);
        }

    }
}
