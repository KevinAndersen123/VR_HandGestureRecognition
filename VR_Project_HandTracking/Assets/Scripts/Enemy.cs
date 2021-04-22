using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int m_health = 100;
    public Slider m_healthBar;
    public Text m_healthText;
    private Vector3 m_targetPos;
    [SerializeField]
    private float m_speed = 3.0f;
    //private float deathTimer;
    private WaveManager m_waveManager;

    [SerializeField]
    GameObject m_bloodParticlePrefab;

    Animator anim;
    private bool m_isAttacking = false;

    private bool m_isDead = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 1);
        setRigidbodyState(true);
        setColliderState(false);
        GetComponent<Animator>().enabled = true;

        m_waveManager = FindObjectOfType<WaveManager>();
        Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        m_targetPos = new Vector3(pos.x, 0.45f, pos.z);
        transform.LookAt(m_targetPos);
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
        m_healthBar.value = m_health;
        m_healthText.text = m_health + "/100";

        if (!m_isAttacking && !m_isDead)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            m_targetPos = new Vector3(pos.x, 0.45f, pos.z);
            transform.LookAt(m_targetPos);
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_speed * Time.deltaTime);
        }

        if(!FindObjectOfType<Player>().m_forcefield.activeSelf && !m_isAttacking)
        {
            m_speed = 0.5f;
            anim.SetInteger("Condition", 1);
        }
 
    }
    public void TakeDamage(int t_val)
    {
        m_health -= t_val;
        if(m_health <= 0)
        {
            Die();
        }
    }

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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            m_isAttacking = false;
            anim.SetInteger("Condition", 1);
        }
    }

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

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;

    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    IEnumerator Attack()
    {
        anim.SetInteger("Condition", 2);
        yield return new WaitForSeconds(2.0f);
        FindObjectOfType<Player>().TakeDamage(0.02f);
    }
}
