using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            EventManager.TriggerEvent(Const.Events.LevelCompleted);
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerManager.godMode = !PlayerManager.godMode;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerManager.oneShotMode = !PlayerManager.oneShotMode;
        }
    }
}
