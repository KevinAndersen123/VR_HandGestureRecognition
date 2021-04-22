using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private float m_timerVal;
    private bool m_runTimer;
    public Text m_timerText;
    public UnityEvent m_timerEvent;

    private void Start()
    {
        HideUI();
    }

    public void StartTimer(float t_val)
    {
        m_timerVal = t_val;
        m_runTimer = true;
    }

    public void StopTimer()
    {
        m_runTimer = false;
        HideUI();
    }

    private void Update()
    {
        if(m_runTimer)
        {
            m_timerVal -= Time.deltaTime;
            UpdateUI();

            if(m_timerVal <= 0f)
            {
                StopTimer();
                if(m_timerEvent != null)
                {
                    m_timerEvent.Invoke();
                }
            }
        }
    }

    private void UpdateUI()
    {
        if(m_timerVal <=1.0f)
        {
            m_timerText.text = "For Odin!";
        }
        else
        {
            m_timerText.text = m_timerVal.ToString("0");
        }
    }

    private void HideUI()
    {
        m_timerText.text = "";
    }

}
