using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    private void Start()
    {
        EventManager.TriggerEvent(Const.Events.InCutScene);
    }

    public void StartGameButton()
    {
        EventManager.TriggerEvent(Const.Events.CutSceneCompleted);
        UnityEngine.SceneManagement.SceneManager.LoadScene(Const.SceneNames.Tutorial);
    }
}
