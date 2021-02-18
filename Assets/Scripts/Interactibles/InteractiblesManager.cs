using UnityEngine;

public class InteractiblesManager : MonoBehaviour
{
    private static GameObject _interactible;

    public static void SpawnInteractible(Vector3 position, Quaternion rotation)
    {
        if(GameAssets.i.GenerateRandomNumber(0,2) == 2)
        {
            Instantiate(SelectInteractible(), position, rotation);
        }
        
    }

    private static GameObject SelectInteractible()
    {
        switch (GameAssets.i.GenerateRandomNumber(0,2))
        {
            case 0:
                _interactible = Resources.Load("Prefabs/box_ammo") as GameObject;
                break;
            case 1:
                _interactible = Resources.Load("Prefabs/box_med") as GameObject;
                break;
            case 2:
                _interactible = Resources.Load("Prefabs/box_supply") as GameObject;
                break;
        }
        return _interactible;
    }

    
}
