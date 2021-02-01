using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private UnityEngine.UI.Text waveUI;
    private string waveCompletedText = "Wave clear";
    private string levelCompletedText = "Level completed";
    private int waveCounter = 1;
    private string waveCountText;
    private string emptyString = "";
    private float showUITime = 5;


    void Start()
    {     
        waveUI = GameObject.FindGameObjectWithTag("WaveUI").GetComponent<UnityEngine.UI.Text>();
        ShowWaveText();
        EventManager.StartListening(Const.Events.WaveCompleted, ShowCompleteText);
        EventManager.StartListening(Const.Events.ResumeSpawningZombies, ShowWaveText);
        EventManager.StartListening(Const.Events.LevelCompleted, ShowLevelCompletedText);
    }

    private void ShowCompleteText()
    {
        waveUI.text = waveCompletedText;
		SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Wave) as AudioClip);
        StartCoroutine(waitTime());    
    }

    private void ShowWaveText()
    {
        waveCountText = "Wave " + waveCounter;
		SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Wave) as AudioClip);
        waveUI.text = waveCountText;
        StartCoroutine(waitTime());
        waveCounter++;
    }

    private void ShowLevelCompletedText()
    {
        waveUI.text = levelCompletedText;
		SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.Wave) as AudioClip);
        StartCoroutine(waitTime());
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(showUITime);
        waveUI.text = emptyString;
    }
}
