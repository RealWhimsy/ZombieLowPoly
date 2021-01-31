using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(Const.Events.LevelCompleted, LoadNextLevel);
        EventManager.StartListening(Const.Events.PlayerDead, ReloadLevel);
    }


    private void LoadNextLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case Const.SceneNames.Tutorial:
                StartCoroutine(LoadLevel(Const.SceneNames.Forest));
                break;
            case Const.SceneNames.Forest:
                StartCoroutine(LoadLevel(Const.SceneNames.Desert));
                break;
            case Const.SceneNames.Desert:
                StartCoroutine(LoadLevel(Const.SceneNames.PirateBay));
                break;
            case Const.SceneNames.PirateBay:
                StartCoroutine(LoadLevel(Const.SceneNames.City));
                break;
        }
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(sceneName);
    }

    private void ReloadLevel()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadLevel(activeScene));
    }

    
}
