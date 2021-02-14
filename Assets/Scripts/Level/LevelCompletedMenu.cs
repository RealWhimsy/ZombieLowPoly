using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletedMenu : MonoBehaviour
{
    public void ButtonSelected(int difficultyAdjustment)
    {
        DDAManager.PlayerFeedbackDifficulty = difficultyAdjustment;
        EventManager.TriggerEvent(Const.Events.DifficultySelected);
    }
}