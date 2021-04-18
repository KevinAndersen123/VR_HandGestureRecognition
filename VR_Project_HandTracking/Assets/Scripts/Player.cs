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
    private int m_maxHealth = 100;
    public float m_health = 100;
    bool isRegenHealth;
    PlayerState m_state = PlayerState.Idle;
    [SerializeField]
    Image m_hud;
    [SerializeField]
    GameObject m_shield;

    [SerializeField]
    GameObject m_axe;

    [SerializeField]
    GameObject m_lightningFX;

    ShootManager m_shootManager;
    bool m_lightningActive = false;
    private GameManager m_manager;

    private void Start()
    {
        m_manager = FindObjectOfType<GameManager>();
        m_shootManager = FindObjectOfType<ShootManager>();
        m_hud.color = Color.clear;
        m_lightningFX.SetActive(false);
        m_health = m_maxHealth;
    }

    void Update()
    {
        if (m_health != m_maxHealth && !isRegenHealth)
        {
            StartCoroutine(RegainHealthOverTime());
        }
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
                m_shield.SetActive(false);
                m_shootManager.StopShoot();
                m_lightningFX.SetActive(false);
                m_lightningActive = false;
                m_axe.SetActive(false);
                break;
            case "Draw":
                m_state = PlayerState.Drawing;
                break;
            case "Circle":
                m_state = PlayerState.Shield;
                m_shield.SetActive(true);
                break;
            case "Lightning":
                m_state = PlayerState.Lightning;
                m_lightningFX.SetActive(true);
                if (!m_lightningActive)
                {
                    FindObjectOfType<AudioManager>().Play("Thunder");
                }
                m_lightningActive = true;
                break;
            case "Line":
                SwitchState("Idle");
                break;
            default:
                break;
        }
    }

    public void Axe()
    {
        m_axe.SetActive(true);
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
        m_hud.color = new Color(1, 1, 1, (100 - m_health) / 100.0f);
    }

    public void Healthregen()
    {
        m_health++;
        UpdateHealthHUD();
    }
    private IEnumerator RegainHealthOverTime()
    {
        isRegenHealth = true;
        while (m_health < m_maxHealth)
        {
            Healthregen();
            yield return new WaitForSeconds(1);
        }
        isRegenHealth = false;
    }
}
