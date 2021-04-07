using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int m_health = 100;

    private Vector3 m_targetPos;
    [SerializeField]
    private float m_speed = 3.0f;
    //private float deathTimer;
    private WaveManager m_waveManager;
    [SerializeField]
    GameObject m_bloodParticlePrefab;

    Animator anim;
    private bool m_isAttacking = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Condition", 1);
        setRigidbodyState(true);
        setColliderState(false);
        GetComponent<Animator>().enabled = true;

        m_waveManager = FindObjectOfType<WaveManager>();
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        m_targetPos = new Vector3(pos.x, 0.45f, pos.z);
        transform.LookAt(m_targetPos);
    }

    public void FixedUpdate()
    {
        if (!m_isAttacking)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
            m_targetPos = new Vector3(pos.x, 0.45f, pos.z);
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos, m_speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(Attack());
        }
    }
    public void TakeDamage(int t_val)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        m_health -= t_val;
        if(m_health <= 0)
        {
            Debug.Log("HIT ENEMY!");
            Die();
        }
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //public void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        StartCoroutine(Attack());
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_isAttacking = true;
            anim.SetInteger("Condition", 0);
        }
    }
    public void Die()
    {
        m_isAttacking = false;
        anim.SetInteger("Condition", 0);
        GetComponent<Animator>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        if (gameObject != null)
        {
            FindObjectOfType<WaveManager>().EnemyDied();
            Instantiate(m_bloodParticlePrefab, transform);
            Destroy(gameObject, 3f);
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
        FindObjectOfType<Player>().TakeDamage(0.05f);
        yield return new WaitForSeconds(1.0f);
    }
}
