using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //list of gameobject
    public List<GameObject> objects;

    //enables the gameobject with the name of the passed in param
    public void Spawn(string objectName)
    {
        foreach(var obj in objects)
        {
            obj.SetActive(objectName == obj.name);
        }

    }
}
