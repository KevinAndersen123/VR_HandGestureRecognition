using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{ 
    Idle,
    Drawing,
    Shield,
    Lightning,
    Axe
}


public class Player : MonoBehaviour
{
    PlayerState m_state = PlayerState.Idle;

    [SerializeField]
    GameObject m_shield;

    [SerializeField]
    GameObject m_leftHand;

    [SerializeField]
    GameObject m_rightHand;

    [SerializeField]
    GameObject m_lightningFX;

    private void FixedUpdate()
    {
        switch (m_state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Drawing:
                break;
            case PlayerState.Shield:
                Shield();
                break;
            case PlayerState.Lightning:
                Lightning();
                break;
            case PlayerState.Axe:
                break;
        }

        if(m_state == PlayerState.Lightning)
        {
            m_lightningFX.SetActive(true);
        }
        else
        {
            m_lightningFX.SetActive(false);
        }
    }

    public void Shield()
    {
        Instantiate(m_shield, m_leftHand.transform.position, m_leftHand.transform.rotation);
        SwitchState("Idle");
    }

    public void Lightning()
    {
        m_lightningFX.SetActive(true);
    }

    public void SwitchState(string t_state)
    {
        switch (t_state)
        {
            case "Idle":
                m_state = PlayerState.Idle;
                break;
            case "Draw":
                m_state = PlayerState.Drawing;
                break;
            case "Circle":
                m_state = PlayerState.Shield;
                break;
            case "Lightning":
                m_state = PlayerState.Lightning;
                break;
            case "Line":
                m_state = PlayerState.Axe;
                break;
        }
    }
}
