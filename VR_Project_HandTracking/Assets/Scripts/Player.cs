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

    bool shieldAlive = false;
    public bool m_isLightningStrike = false;
    const int shieldStartTime = 15;
    private float m_shieldTime = shieldStartTime;

    private void FixedUpdate()
    {
        switch (m_state)
        {
            case PlayerState.Idle:
                m_isLightningStrike = false;
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

        if (shieldAlive)
        {
            if (m_shieldTime > 0)
            {
                m_shieldTime -= Time.deltaTime;
            }
            else
            {
                m_shield.SetActive(false);
                shieldAlive = false;
                m_shieldTime = shieldStartTime;
            }
        }
    }

    public void Shield()
    {
        if (!shieldAlive)
        {
            shieldAlive = true;
            m_shield.SetActive(true);
        }
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
    public void LightningStrike()
    {
        if(m_state == PlayerState.Lightning)
        {
            m_isLightningStrike = true;
        }
        else
        {
            m_isLightningStrike = false;
        }
    }
}
