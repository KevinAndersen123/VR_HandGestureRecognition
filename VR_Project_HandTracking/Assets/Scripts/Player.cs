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
    GameObject m_lightningFX;
    ShootManager m_shootManager;
    bool m_shieldAlive = false;
    bool m_lightningActive = false;
    const float SHIELD_START_TIME = 15.0f;
    private float m_shieldTime = SHIELD_START_TIME;
    private GameManager m_manager;
    private void Start()
    {
        m_manager = FindObjectOfType<GameManager>();
        m_shootManager = FindObjectOfType<ShootManager>();
        m_hud.color = Color.clear;
        m_lightningFX.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (m_shieldAlive)
        {
            if (m_shieldTime >= 0f)
            {
                m_shieldTime -= Time.deltaTime;
            }
            else
            {
                m_shield.SetActive(false);
                m_shieldAlive = false;
                m_shieldTime = SHIELD_START_TIME;
            }
        }
    }

    public void Shield()
    {
        if (!m_shieldAlive)
        {
            m_shieldAlive = true;
            m_shield.SetActive(true);
        }
        SwitchState("Idle");
    }

    public void ShootLightning()
    {
        if (m_state == PlayerState.Lightning)
        {
            m_shootManager.OnShoot();
        }
    }
    public void SwitchState(string t_state)
    {
        switch (t_state)
        {
            case "Idle":
                m_state = PlayerState.Idle;
                m_shootManager.StopShoot();
                m_lightningFX.SetActive(false);
                m_lightningActive = false;
                break;
            case "Draw":
                m_state = PlayerState.Drawing;
                break;
            case "Circle":
                m_state = PlayerState.Shield;
                Shield();
                break;
            case "Lightning":
                m_state = PlayerState.Lightning;
                m_lightningFX.SetActive(true);
                if(!m_lightningActive)
                {
                    FindObjectOfType<AudioManager>().Play("Thunder");
                }
                m_lightningActive = true;
                break;
            case "Line":
                m_state = PlayerState.Axe;
                break;
        }
    }

    public void TakeDamage(float t_damage)
    {
        FindObjectOfType<AudioManager>().Play("Player_Hurt");
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
