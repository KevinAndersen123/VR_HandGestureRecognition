using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
public class MovementRecognizer : MonoBehaviour
{
    [SerializeField]
    OVRHand m_hand; //hand that the pinching will be preformed by
    public float pinchThreshold = 0.7f; //threshold for how accurate the pinching must be

    private bool isMoving = false;  //if hand is moving or not
    public Transform movementSource; //source of the movement
    public float newPosThresholdDist = 0.05f; //threshold to create a new point position from last
    private List<Vector3> posList = new List<Vector3>(); //list of positions

    public bool creationMode = false; //if creating new gesture or not
    public string newGestureName;   //name of gesture

    private List<Gesture> trainingSet = new List<Gesture>(); //list of trained gestures

    public GameObject drawPrefab; //particle effects for points drawn

    public float recognitionThreshold = 0.8f; //threshold for accuracy of recognition
    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { } 
    public UnityStringEvent OnRecognized; //string event for actions to do after gesture is recognized
    bool endOnce = false; //if movement has ended
    void Start()
    {
        m_hand = GetComponent<OVRHand>();
        //finds all datapath that ends with xml
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");

        //add gestures to training set
        foreach (var file in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(file));
        }
    }

    //start drawing point clouds and adding their pos to the list of positions
    public void StartMovement()
    {
        endOnce = true;
        posList.Clear();
        //Debug.Log("S");
        isMoving = true;
        posList.Add(movementSource.position);
        if (drawPrefab)
            Destroy(Instantiate(drawPrefab, movementSource.position, Quaternion.identity), 3);
    }

    //check if the player is pinching
    public void Update()
    { 
        CheckIndexPinch();
    }

    public void EndMovement()
    {
        endOnce = true;
        //Debug.Log("E");
        isMoving = false;

        //Creates gesture from the position list
        Point[] pointArray = new Point[posList.Count];

        //convert each position to a 2d point
        for (int i = 0; i < posList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(posList[i]);

            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        //if a new gesture created, save it to xml file
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

            //invoke the action if over threshold value
            if (result.Score > recognitionThreshold)
            {
                OnRecognized?.Invoke(result.GestureClass);
            }
        }
    }

    //adds more point clouds and spawns the particle effect
    public void UpdateMovement()
    {
        Vector3 lastPos = posList[posList.Count - 1];
        if (Vector3.Distance(movementSource.position, lastPos) > newPosThresholdDist)
        {
            posList.Add(movementSource.position);
            if (drawPrefab)
                Destroy(Instantiate(drawPrefab, movementSource.position, Quaternion.identity), 3);
        }
        //Debug.Log("U");
    }

    //checks if the player is pinching or not
    //Decides if to start, update or end the movement of the gesture points
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
