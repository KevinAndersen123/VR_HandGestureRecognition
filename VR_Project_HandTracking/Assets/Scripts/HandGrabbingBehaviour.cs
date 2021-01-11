using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

//@Author: Kevin Andersen
public class HandGrabbingBehaviour : OVRGrabber
{
    private OVRHand m_hand;
    public float pinchThreshold = 0.7f;

    protected override void Start()
    {
        base.Start();
        m_hand = GetComponent<OVRHand>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = GetComponent<OVRHand>().GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        if (!m_grabbedObj && isPinching && m_grabCandidates.Count > 0)
        {
            GrabBegin();
        }
        else if (m_grabbedObj && !isPinching)
        {
            GrabEnd();
        }
    }
}