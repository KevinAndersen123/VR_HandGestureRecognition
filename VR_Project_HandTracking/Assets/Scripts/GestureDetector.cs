using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerData;
    public UnityEvent onRecognized;

}

public class GestureDetector : MonoBehaviour
{
    public float threshold = 0.1f;
    public OVRSkeleton skeleton;
    public List<Gesture> gestures;
    private List<OVRBone> fingerBones;
    public bool debugMode = true;
    private Gesture previousGesture;

    IEnumerator GetFingerBones()
    {
        do
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
            yield return null;
        } while (fingerBones.Count <= 0);
    }

    void Start()
    {
        StartCoroutine(GetFingerBones());
        //fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && debugMode && fingerBones.Count > 0)
        {
            Save();
        }

        Gesture currentGesture = RecogniseHandGesture();
        bool hasRecognised = !currentGesture.Equals(new Gesture());
        //checks if new gesture
        if (hasRecognised && !currentGesture.Equals(previousGesture))
        {
            Debug.Log("New Gesture Found: " + currentGesture.name);
            previousGesture = currentGesture;
            currentGesture.onRecognized.Invoke();
        }

    }

    void Save()
    {
        Gesture g = new Gesture();
        g.name = "New Gesture";
        List<Vector3> data = new List<Vector3>();

        foreach (var bone in fingerBones)
        {
            //finger pos relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }
        g.fingerData = data;
        gestures.Add(g);
    }

    Gesture RecogniseHandGesture()
    {
        Debug.Log("Recognising");
        Gesture currentGesture = new Gesture();
        float currentMIn = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerData[i]);
                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }
                sumDistance += distance;
            }
            if (!isDiscarded && sumDistance < currentMIn)
            {
                currentMIn = sumDistance;
                currentGesture = gesture;
            }
        }
        return currentGesture;
    }
}
