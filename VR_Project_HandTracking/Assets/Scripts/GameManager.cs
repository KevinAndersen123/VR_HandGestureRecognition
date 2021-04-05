using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private WaveManager m_waveManager;
    public bool m_gameover = false;

    private void Start()
    {
        m_waveManager = GetComponent<WaveManager>();
        m_waveManager.StartWave();
    }
}

