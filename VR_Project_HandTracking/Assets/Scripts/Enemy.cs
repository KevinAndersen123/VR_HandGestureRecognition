using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int m_health = 100;              //health of the enemy
    public Slider m_healthBar;              //slider gameobject 
    public Text m_healthText;               //slider text
    private Vector3 m_targetPos;            //the target position of the enemy

    [SerializeField]                        
    private float m_speed = 3.0f;           //speed of the enemy

    private WaveManager m_waveManager;      //refernce to the wave manager

    [SerializeField]
    GameObject m_bloodParticlePrefab;       //blood particle system prefab

    Animator anim;                          //animator for the enemy object
    private bool m_isAttacking = false;     //if the enemy is attacking or not

    private bool m_isDead = false;          //if the enemy is dead or not

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 1); //sets animation to walk
        setRigidbodyState(true);     
        setColliderState(false);
        GetComponent<Animator>().enabled = true; //enable the animator

        m_waveManager = FindObjectOfType<WaveManager>();
        Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position; //get the position of the camera(player) in the scene and get its position
        m_targetPos = new Vector3(pos.x, 0.45f, pos.z); //set the target position to the player
        transform.LookAt(m_targetPos); //rotate towards the player

        //change speed based on the wave number
        if (m_waveManager.GetCurrentWave() > 4 && m_waveManager.GetCurrentWave() < 10)
        {
            m_speed = 2f;
        }
        else if (m_waveManager.GetCurrentWave() > 10)
        {
            m_speed = 4.0f;
        }
    }

    public void FixedUpdate()
    {
        //updates health bar slider and text
        m_healthBar.value = m_health;
        m_healthText.text = m_health + "/100";

        //move towards the player
        if (!m_isAttacking && !m_isDead)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            m_targetPos = new Vector3(pos.x, 0.45f, pos.z);
            transform.LookAt(m_targetPos);
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_speed * Time.deltaTime);
        }
        //if there is no forcefield active and the enmy is not attacking, continue walking towards player
        if(!FindObjectOfType<Player>().m_forcefield.activeSelf && !m_isAttacking)
        {
            m_speed = 0.5f;
            anim.SetInteger("Condition", 1);
        }
 
    }
    /// <summary>
    /// Decreases health based on passed in param
    /// </summary>
    /// <param name="t_val"> value of how much damage the enemy will take</param>
    public void TakeDamage(int t_val)
    {
        m_health -= t_val;
        if(m_health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Checks if the enemy eneters the players area or hits force field
    /// Changes animation based on what the enemy collides with
    /// </summary>
    /// <param name="other">collider of trigger</param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            m_isAttacking = true;
            anim.SetInteger("Condition", 2);
        }
        if (other.tag == "Shield")
        {
            m_speed = 0;
            anim.SetInteger("Condition", 0);
        }
    }

    /// <summary>
    /// Checks for collisions with a lightning bolt or axe
    /// Takes damage if hit by one
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lightning")
        {
            TakeDamage((int)collision.gameObject.GetComponent<LightningBolt>().m_damage);
        }
        if (collision.gameObject.tag == "Axe")
        {
            TakeDamage(100);
        }
    }
    /// <summary>
    /// If the enemy is still within the players area, continue to attack
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            StartCoroutine(Attack());
        }
    }
    /// <summary>
    /// If the enemy leaves the players area, move towards the player again
    /// Set animation to walking
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            m_isAttacking = false;
            anim.SetInteger("Condition", 1);
        }
    }
    /// <summary>
    /// Enables the ragdoll effect
    /// Disables animator and instantiates the blood particle effect
    /// Plays the hurt and blood sound effects
    /// </summary>
    public void Die()
    {
        m_isDead = true;
        m_isAttacking = false;
        anim.SetInteger("Condition", 0);
        GetComponent<Animator>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        if (gameObject != null)
        {
            FindObjectOfType<AudioManager>().Play("Enemy_Hurt");
            FindObjectOfType<WaveManager>().EnemyDied();
            Destroy(gameObject, 3f);
            Instantiate(m_bloodParticlePrefab, transform);
            FindObjectOfType<AudioManager>().Play("Blood");
        }
    }

    /// <summary>
    /// Disables or enables the rigidbodies in the gameobjects in its children
    /// </summary>
    /// <param name="state"></param>
    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;

    }

    /// <summary>
    /// Sets the colliders of the children to the passed in boolean
    /// </summary>
    /// <param name="state"></param>
    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    //Continues to attack the player every 2 seconds, dealing damage each time
    IEnumerator Attack()
    {
        anim.SetInteger("Condition", 2);
        yield return new WaitForSeconds(2.0f);
        FindObjectOfType<Player>().TakeDamage(0.02f);
    }
}
