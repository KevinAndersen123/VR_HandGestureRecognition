using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
public class MovementRecognizer : MonoBehaviour
{
    [SerializeField]
    OVRHand m_hand;
    public float pinchThreshold = 0.7f;

    private bool isMoving = false;
    public Transform movementSource;
    public float newPosThresholdDist = 0.05f;
    private List<Vector3> posList = new List<Vector3>();

    public bool creationMode = false;
    public string newGestureName;

    private List<Gesture> trainingSet = new List<Gesture>();

    public GameObject drawPrefab;

    public float recognitionThreshold = 0.8f;
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognized;
    bool endOnce = false;
    void Start()
    {
        m_hand = GetComponent<OVRHand>();
        //finds all datapath that ends with xml
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");

        foreach (var file in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(file));
        }
    }
    public void StartMovement()
    {
        endOnce = true;
        posList.Clear();
        Debug.Log("S");
        isMoving = true;
        posList.Add(movementSource.position);
        if (drawPrefab)
            Destroy(Instantiate(drawPrefab, movementSource.position, Quaternion.identity), 3);
    }

    public void Update()
    {
        CheckIndexPinch();
    }

    public void EndMovement()
    {
        endOnce = true;
        Debug.Log("E");
        isMoving = false;

        //Creates gesture from the position list

        Point[] pointArray = new Point[posList.Count];
        for (int i = 0; i < posList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(posList[i]);

            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        //recognize
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + result.Score);

            if (result.Score > recognitionThreshold)
            {
                OnRecognized?.Invoke(result.GestureClass);
            }
        }
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

    void CheckIndexPinch()
    {
        float pinchStrength = GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        if(isPinching)
        {
            if (!isMoving)
            {
                StartMovement();
            }
            else
            {
                UpdateMovement();
            }
        }
        else
        {
            if(endOnce)
            {
                endOnce = false;
                EndMovement();
            }
        }
    }
}
