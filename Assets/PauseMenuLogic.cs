using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    private static bool _paused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_paused)
            {
                ActivatePauseMenu();
            }
            else
            {
                DeactivatePauseMenu();
            }
        }
    }

    public void DeactivatePauseMenu()
    {
        _paused = !_paused;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // resume game
    }

    private void ActivatePauseMenu()
    {
        _paused = !_paused;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // pause the game
    }

    public static bool Paused => _paused;
}
