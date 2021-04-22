using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text m_waveText;
    public Text m_enemyText;

    public void UpdateUI(int t_currentWave, int t_enemiesLeft)
    {
        m_waveText.text = "Wave " + t_currentWave.ToString();
        m_enemyText.text = "Enemies Left: " + t_enemiesLeft.ToString();
    }

    public void HideUi()
    {
        m_waveText.text = "";
        m_enemyText.text = "";
    }
}