using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    private int killCounter;

    private GameObject followCamera;
    private GameObject player;
    private TextMeshPro tutorialText;
    private GameObject[] targets;

    private string movementText = "Move around using 'WASD'";

    private string shootingText = "Shoot at your enemies with 'LMB'";

    private string grenadeText = "Throw grenades with 'G'";

    private string pickUpText = "Pickup weapons using 'E'";

    private string meleeText = "To melee attack press 'Q'";

    private string skillText = "Kill remaining enemies to complete mission!";
    private double trigger2 = 20.0;
    private double trigger3 = 30.0;
    private double trigger4 = 45.0;
    private double trigger5 = 56.0;
    private double trigger6 = 76.0;
    private double endTrigger = 95.0;
    private bool loading = false;

    void Start()
    {
        killCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        targets = GameObject.FindGameObjectsWithTag("Target");
        followCamera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        tutorialText =  GetComponent<TextMeshPro>();
        tutorialText.text = movementText;
    }

    // Update is called once per frame
    void Update()
    {
        tutorialText.transform.position = new Vector3(followCamera.transform.position.x + 5, 3, followCamera.transform.position.z + 8);
        tutorialText.transform.rotation = followCamera.transform.rotation;
        setTextForPosition();
        killCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void setTextForPosition()
    {
        if (player.transform.position.x >= trigger2)
        {
            tutorialText.text = pickUpText;
            targets[0].SetActive(false);
        }
        
        if (player.transform.position.x >= trigger3)
        {
            tutorialText.text = shootingText;
        }

        if (player.transform.position.x >= trigger4)
        {
            tutorialText.text = grenadeText;
        }
        
        if (player.transform.position.x >= trigger5)
        {
            tutorialText.text = meleeText;
        }
        
        if (player.transform.position.x >= trigger6)
        {
            tutorialText.text = skillText;
        }
    }
}
