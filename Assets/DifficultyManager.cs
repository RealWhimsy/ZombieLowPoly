using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    void Awake()
    {
        EventManager.StartListening(Const.Events.DifficultyChanged, HandleDifficultyChange);
    }

    private void HandleDifficultyChange()
    {
        Difficulty.CurrentDifficultyIndex += 1;

        // get difficulty at index from Dictionary
        Difficulty.CurrentDifficulty = DifficultyStats.difficulties.ElementAt(Difficulty.CurrentDifficultyIndex).Value;
    }

  
}
