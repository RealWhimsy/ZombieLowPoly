using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiblesManager : MonoBehaviour
{

    private System.Random rand = new System.Random();
    private int selector = -1;
    private int propability = -1;
    private GameObject interactible;

    public void SpawnInteractible()
    {
        propability = rand.Next(0, 3);
        if(propability == 2)
        {
            Instantiate(SelectInteractible(), transform.position, transform.rotation);
        }
        
    }

    private GameObject SelectInteractible()
    {
        interactible = new GameObject();
        selector = rand.Next(0, 3);
        switch (selector)
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
