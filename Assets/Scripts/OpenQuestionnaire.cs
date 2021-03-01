using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenQuestionnaire : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }
    
    public void OpenQuestionnaireButton()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSevtp7kr62cwUn41hAnTHChez6eAZgL3DsziluJO5iCG0XWjw/viewform?entry.393019152=" + LoggingManager.PlayerId);
    }
}
