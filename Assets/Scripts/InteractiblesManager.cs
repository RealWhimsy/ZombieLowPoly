using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiblesManager : MonoBehaviour
{

    private System.Random rand = new System.Random();
    private GameObject interactible;

    public void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
    }

    public void SpawnInteractible()
    {
        if(GameAssets.i.GenerateRandomNumber(0,3) == 2)
        {
            Instantiate(SelectInteractible(), transform.position, transform.rotation);
        }
        
    }

    private GameObject SelectInteractible()
    {
        //interactible = new GameObject();
        switch (GameAssets.i.GenerateRandomNumber(0,2))
        {
            case 0:
                interactible = Resources.Load("Prefabs/box_ammo") as GameObject;
                break;
            case 1:
                interactible = Resources.Load("Prefabs/box_med") as GameObject;
                break;
            case 2:
                interactible = Resources.Load("Prefabs/box_supply") as GameObject;
                break;
        }
        return interactible;
    }

    
}
