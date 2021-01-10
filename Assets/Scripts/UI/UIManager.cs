using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private GameObject waveUI;
    private string waveCompletedText = "Wave clear";
    private string levelCompletedText = "Level completed";
    private int waveCounter = 1;
    private string waveCountText;
    private string emptyString = "";
    private float showUITime = 5;


    void Start()
    {     
        waveUI = GameObject.FindGameObjectWithTag("WaveUI");
        ShowWaveText();
        EventManager.StartListening(Const.Events.WavePassed, ShowCompleteText);
        EventManager.StartListening(Const.Events.ResumeSpawningZombies, ShowWaveText);
        EventManager.StartListening(Const.Events.LevelCompleted, ShowLevelCompletedText);
    }

    private void ShowCompleteText()
    {
        waveUI.GetComponent<UnityEngine.UI.Text>().text = waveCompletedText;
        StartCoroutine(waitTime());    
    }

    private void ShowWaveText()
    {
        waveCountText = "Wave " + waveCounter;
        waveUI.GetComponent<UnityEngine.UI.Text>().text = waveCountText;
        StartCoroutine(waitTime());
        waveCounter++;
    }

    private void ShowLevelCompletedText()
    {
        waveUI.GetComponent<UnityEngine.UI.Text>().text = levelCompletedText;
        StartCoroutine(waitTime());
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(showUITime);
        waveUI.GetComponent<UnityEngine.UI.Text>().text = emptyString;
    }
}
