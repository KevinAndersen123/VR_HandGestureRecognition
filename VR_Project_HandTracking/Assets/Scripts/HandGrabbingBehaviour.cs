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

    protected override void GrabEnd()
    {
        if(m_grabbedObj)
        {
            Vector3 linearVelocity= (transform.position - m_lastPos) / Time.fixedDeltaTime;
            Vector3 angularVelocity = (transform.eulerAngles - m_lastRot.eulerAngles) / Time.fixedDeltaTime;

            GrabbableRelease(linearVelocity, angularVelocity);
        }
        GrabVolumeEnable(true);
    }
}