using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//States of the player
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
    private int m_maxHealth = 100;                      //Max health of the player
    public float m_health = 100;                        //current health of the player
    bool isRegenHealth;                                 //if player is regenerating health
    PlayerState m_state = PlayerState.Idle;             //players state

    [SerializeField]
    Image m_hud;                                        //Hud of the player

    [SerializeField]
    GameObject m_shield;                                //shield Gameobject

    [SerializeField]
    public GameObject m_forcefield;                     //forcefield gameobject

    [SerializeField]
    GameObject m_axe;                                   //axe gameobject

    [SerializeField]
    GameObject m_lightningFX;                           //lightning particle effect

    ShootManager m_shootManager;                        //reference to the shoot manager
    bool m_lightningActive = false;                     //if the lightning is active or not
    private GameManager m_manager;                      //reference to the gamemanger

    const int shieldStartTime = 20;                     //Start time of the shield cooldown
    private float m_shieldTime = shieldStartTime;       //current time of the shield cooldown

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
        //if not at max health, start regaining health
        if (m_health != m_maxHealth && !isRegenHealth)
        {
            StartCoroutine(RegainHealthOverTime());
        }

        //if the shield is active, set the players collider to 0.1
        //reset it if shield is not active
        if(m_shield.activeSelf)
        {
            GetComponent<CapsuleCollider>().radius = 0.1f;
            m_forcefield.SetActive(true);
        }
        else
        {
            GetComponent<CapsuleCollider>().radius = 1f;
            m_forcefield.SetActive(false);
        }

        //cooldown timer for shield, switch to idle state when over
        if (m_shield.activeSelf)
        {
            if (m_shieldTime > 0)
            {
                m_shieldTime -= Time.deltaTime;
            }
            else
            {
                m_shieldTime = shieldStartTime;
                SwitchState("Idle");
            }
        }
        //update the cooldown text of the shield
        m_shield.GetComponentInChildren<Text>().text = m_shieldTime.ToString();
    }

    //shoots lightning bolt if the player state is lightning
    public void ShootLightning()
    {
        if (m_state == PlayerState.Lightning)
        {
            m_shootManager.OnShoot();
        }
    }

    //switch state of the player 
    public void SwitchState(string t_state)
    {
        switch (t_state)
        {
            //idle state: disables all active actions and resets cooldown for shield
            case "Idle":
                m_state = PlayerState.Idle;
                m_shield.SetActive(false);
                m_shootManager.StopShoot();
                m_lightningFX.SetActive(false);
                m_lightningActive = false;
                m_axe.SetActive(false);
                FindObjectOfType<AudioManager>().Stop("Wind");
                FindObjectOfType<AudioManager>().Stop("Lightning");
                m_shieldTime = shieldStartTime;
                break;
            //circle state: Enables the shield and forcefield
            case "Circle":
                m_state = PlayerState.Shield;
                if (!m_shield.activeSelf)
                {
                    FindObjectOfType<AudioManager>().Play("Wind");
                    FindObjectOfType<AudioManager>().Play("Lightning");
                }
                m_shield.SetActive(true);
                m_forcefield.SetActive(true);
                break;
            //lighting state: Activates the ablity to shoot lightning bolts and sets the lightning effect to true
            case "Lightning":
                m_state = PlayerState.Lightning;
                m_lightningFX.SetActive(true);
                if (!m_lightningActive)
                {
                    FindObjectOfType<AudioManager>().Play("Thunder");
                }
                m_lightningActive = true;
                break;
            //line switches state to idle, this is called from the movement gesture "line"
            case "Line":
                SwitchState("Idle");
                break;
            default:
                break;
        }
    }

    //enables axe gameobject
    public void Axe()
    {
        m_axe.SetActive(true);
    }

    //reduces health based on param
    //if health is at 0 or less, switch to gameover scene
    public void TakeDamage(float t_damage)
    {
        FindObjectOfType<AudioManager>().Play("Player_Hurt");
        m_health -= t_damage;
        if(m_health <= 0)
        {
            m_manager.m_gameover = true;
            FindObjectOfType<LevelChanger>().FadeToLevel("Gameover");
        }
        UpdateHealthHUD();
    }

    //changes the alpha value of the blood image on the hud
    private void UpdateHealthHUD()
    {
        m_hud.color = new Color(1, 1, 1, (100 - m_health) / 100.0f);
    }

    //increases health by 1
    public void Healthregen()
    {
        m_health++;
        UpdateHealthHUD();
    }
    //regen health every second
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
