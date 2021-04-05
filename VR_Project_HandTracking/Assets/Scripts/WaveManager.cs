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

    public void Start()
    {
        m_manager = GetComponent<GameManager>();
        m_timer = GetComponent<Timer>();
        m_ui = GetComponent<WaveUI>();
    }

    public void StartWave()
    {
        m_currentWave++;

        m_enemiesInWave = m_enemyIncreaseFactor * m_currentWave; // may change
        m_enemiesToSpawn = m_enemiesInWave;
        GetComponent<WaveUI>().UpdateUI(m_currentWave, m_enemiesInWave);

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for(int i =0; i < m_enemiesToSpawn; i++)
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
        m_ui.UpdateUI(m_currentWave, m_enemiesInWave);
        if(m_enemiesInWave == 0)
        {
            EndWave();
        }
    }

    public Vector3 GetRandownSpawner()
    {
        return m_spawners[Random.Range(0, m_spawners.Count)].transform.position;
    }
}
