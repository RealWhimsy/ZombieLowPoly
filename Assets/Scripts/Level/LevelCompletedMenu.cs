using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedMenu : MonoBehaviour
{
    private GameObject statsValues;
    private string values = DDAManager.ZombiesKilled +
                            "\n" +
                            DDAManager.PlayerDeaths +
                            "\n" +
                            DDAManager.ShotsFired + 
                            "\n" +
                            DDAManager.GrenadesThrown; // TODO: add dda score
    public void Start()
    {
        Time.timeScale = 0;
        statsValues = GameObject.Find("StatsValues");
        statsValues.GetComponent<Text>().text = values + "\n0";
    }

    public void ButtonSelected(int difficultyAdjustment)
    {
        DDAManager.PlayerFeedbackDifficulty = difficultyAdjustment;
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }
}