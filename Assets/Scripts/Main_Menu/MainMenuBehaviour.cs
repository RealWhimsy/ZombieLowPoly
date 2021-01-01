using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlayerSelectionMenu;
    public GameObject ControlsMenu;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayerSelectionButton()
    {
        MainMenu.SetActive(false);
        PlayerSelectionMenu.SetActive(true);
        ControlsMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        MainMenu.SetActive(true);
        PlayerSelectionMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    public void ControlsMenuButton()
    {
        MainMenu.SetActive(false);
        PlayerSelectionMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    public void StartGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Playground");
    }
}
