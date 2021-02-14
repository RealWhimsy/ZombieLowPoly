using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private string nextScene = "";
    void Start()
    {
        EventManager.StartListening(Const.Events.LevelCompleted, LoadLevelCompleted);
        EventManager.StartListening(Const.Events.PlayerDead, ReloadLevel);
        EventManager.StartListening(Const.Events.DifficultySelected, LoadNextScene);
    }


    private void LoadLevelCompleted()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case Const.SceneNames.Tutorial:
                StartCoroutine(LoadLevelWithDelay(Const.SceneNames.LevelCompletedScene));
				nextScene = Const.SceneNames.Forest;
                break;
            case Const.SceneNames.Forest:
                StartCoroutine(LoadLevelWithDelay(Const.SceneNames.LevelCompletedScene));
                nextScene = Const.SceneNames.Desert;
                break;
            case Const.SceneNames.Desert:
                StartCoroutine(LoadLevelWithDelay(Const.SceneNames.LevelCompletedScene));
                nextScene = Const.SceneNames.PirateBay;
                break;
            case Const.SceneNames.PirateBay:
                StartCoroutine(LoadLevelWithDelay(Const.SceneNames.LevelCompletedScene));
                nextScene = Const.SceneNames.City;
                break;
        }
    }

    private void LoadNextScene()
    {
        EventManager.TriggerEvent(Const.Events.LevelLoaded);
        switch (nextScene)
        {
			case Const.SceneNames.Forest:
                EventManager.TriggerEvent(Const.Events.TutorialCompleted);
				LoadLevel(Const.SceneNames.Forest);
				break;	
            case Const.SceneNames.Desert:
                LoadLevel(Const.SceneNames.Desert);
                break;
            case Const.SceneNames.PirateBay:
                LoadLevel(Const.SceneNames.PirateBay);
                break;
            case Const.SceneNames.City:
                LoadLevel(Const.SceneNames.City);
                break;
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

	private IEnumerator LoadLevelWithDelay(string sceneName) {
		yield return new WaitForSeconds(4);
		SceneManager.LoadScene(sceneName);
	}

    private void ReloadLevel()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadLevelWithDelay(activeScene));
    }

    
}
