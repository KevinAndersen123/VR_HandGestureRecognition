using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float m_health = 100f;
    PlayerState m_state = PlayerState.Idle;
    [SerializeField]
    Image m_hud;
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
    private GameManager m_manager;
    private void Start()
    {
        m_manager = FindObjectOfType<GameManager>();
        m_hud.color = Color.clear;
    }
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

    public void TakeDamage(float t_damage)
    {
        m_health -= t_damage;
        Debug.Log(m_health);
        if(m_health <= 0)
        {
            m_manager.m_gameover = true;
            FindObjectOfType<LevelChanger>().FadeToLevel("Gameover");
        }
        UpdateHealthHUD();
    }

    private void UpdateHealthHUD()
    {
        if(m_health <=100 && m_health > 75)
        {
            m_hud.color = new Color(1, 1, 1, 0.25f);
        }
        else if (m_health <= 75 && m_health > 50)
        {
            m_hud.color = new Color(1, 1, 1, 0.5f);
        }
        else if (m_health <= 50 && m_health > 25)
        {
            m_hud.color = new Color(1, 1, 1, 0.75f);
        }
        else if(m_health <= 25 && m_health > 1)
        {
            m_hud.color = new Color(1, 1, 1, 0.9f);
        }
        else
        {
            m_hud.color = new Color(1, 1, 1, 1f);
        }

    }
}
