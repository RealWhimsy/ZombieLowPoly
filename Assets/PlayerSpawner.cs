using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerMovement playerMovement;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");

        if (playerSpawn != null)
        {
            transform.position = playerSpawn.transform.position;
            transform.rotation = playerSpawn.transform.rotation;
        }

        // Add all "combat scripts" to the player if the first combat scene is loaded
        if (scene.name.Equals(Const.SceneNames.Forest))
        {
            playerManager = gameObject.AddComponent<PlayerManager>();
            playerMovement = gameObject.AddComponent<PlayerMovement>();
            gameObject.AddComponent<PlayerWeaponInteraction>();
            gameObject.AddComponent<WeaponBehaviour>();
            transform.Find("MeleeArea").gameObject.SetActive(true);

            GameObject.Find("BulletSpawner").AddComponent<BulletSpawner>();

            SetInitialScriptValues();
        }
    }

    private void SetInitialScriptValues()
    {
        playerMovement.moveSpeed = 0.04f;
        playerMovement.anim = GameObject.FindGameObjectWithTag("PlayerModel").GetComponent<Animator>();
    }
}
