using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlayerSelectionMenu;
    public GameObject ControlsMenu;
    public GameObject CreditsMenu;
    public GameObject[] playerModels;
    
    private GameObject playerParent;
    // Start is called before the first frame update
    void Start()
    {
        playerParent = GameObject.Find("PlayerParent");
        MainMenuButton();
    }

    public void PlayerSelectionButton()
    {
        MainMenu.SetActive(false);
        PlayerSelectionMenu.SetActive(true);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        MainMenu.SetActive(true);
        PlayerSelectionMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void ControlsMenuButton()
    {
        MainMenu.SetActive(false);
        PlayerSelectionMenu.SetActive(false);
        ControlsMenu.SetActive(true);
        CreditsMenu.SetActive(false);
    }

    public void CreditsMenuButton()
    {
        MainMenu.SetActive(false);
        PlayerSelectionMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }

    public void StartGameButton(int prefabIndex)
    {
        GameObject playerModel = Instantiate(playerModels[prefabIndex], playerParent.transform, false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("CutsceneIntro");
    }
}
