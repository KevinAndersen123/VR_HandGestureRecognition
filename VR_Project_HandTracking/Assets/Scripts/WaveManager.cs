using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(WaveUI))]
public class WaveManager : MonoBehaviour
{
    private int m_currentWave = 0;
    private int m_enemiesInWave;
    private int m_enemiesToSpawn;

    private WaveUI m_ui;
    private Timer m_timer;
    private GameManager m_manager;

    [SerializeField]
    private List<GameObject> m_spawners = new List<GameObject>();

    public float m_spawnDelay;
    public float m_roundDelay;
    public int m_enemyIncreaseFactor;
    public GameObject m_enemyPrefab;
    float m_uiTime = 2.5f;
    bool m_showUi = true;
    public void Start()
    {
        m_manager = GetComponent<GameManager>();
        m_timer = GetComponent<Timer>();
        m_ui = GetComponent<WaveUI>();
    }

    private void Update()
    {
        if (m_showUi)
        {
            GetComponent<WaveUI>().UpdateUI(m_currentWave, m_enemiesInWave);
            if (m_uiTime > 0)
            {
                m_uiTime -= Time.deltaTime;
            }
            else
            {
                m_showUi = false;
                m_uiTime = 2.5f;
            }
        }
        else
        {
            GetComponent<WaveUI>().HideUi();
        }
    }

    public void StartWave()
    {
        m_currentWave++;
        if(m_currentWave > 5)
        {
            m_spawnDelay = 5;
        }
        if (m_currentWave > 10)
        {
            m_spawnDelay = 3;
        }
        m_enemiesInWave = m_enemyIncreaseFactor * m_currentWave;
        m_enemiesToSpawn = m_enemiesInWave;
        m_showUi = true;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i =0; i < m_enemiesToSpawn; i++)
        {
            if(GetComponent<GameManager>().m_gameover)
            {
                break;
            }
            Instantiate(m_enemyPrefab, GetRandownSpawner(), Quaternion.identity);
            yield return new WaitForSeconds(m_spawnDelay);
        }
    }

    public void EndWave()
    {
        m_timer.StartTimer(m_roundDelay);
    }

    public void EnemyDied()
    {
        m_enemiesInWave--;
        m_showUi = true;
        if (m_enemiesInWave == 0)
        {
            EndWave();
        }
    }

    public Vector3 GetRandownSpawner()
    {
        return m_spawners[Random.Range(0, m_spawners.Count)].transform.position;
    }

    public int GetCurrentWave()
    {
        return m_currentWave;
    }
}
