using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedMenu : MonoBehaviour
{
    void Start()
    {
    }

    public void TooEasyButton()
    {
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }

    public void EasyButton()
    {
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }

    public void MediumButton()
    {
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }

    public void HardButton()
    {
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }
    
    public void TooHardButton()
    {
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }
}