using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRecognizer : MonoBehaviour
{
    private bool isMoving = false;
    public Transform movementSource;
    public float newPosThresholdDist = 0.05f;
    private List<Vector3> posList = new List<Vector3>();

    public GameObject drawPrefab;

    public void StartMovement()
    {
        posList.Clear();
        Debug.Log("S");
        isMoving = true;
        posList.Add(movementSource.position);
        if (drawPrefab)
            Destroy(Instantiate(drawPrefab, movementSource.position, Quaternion.identity),3);
    }

    public void EndMovement()
    {
        Debug.Log("E");
        isMoving = false;
    }

    public void UpdateMovement()
    {
        Vector3 lastPos = posList[posList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPos) > newPosThresholdDist)
        {
            posList.Add(movementSource.position);
            if (drawPrefab)
                Destroy(Instantiate(drawPrefab, movementSource.position, Quaternion.identity), 3);
        }
        Debug.Log("U");
    }
}
