using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        string tag = gameObject.tag;
        if (GameObject.FindGameObjectsWithTag(tag).Length > 1)
        {
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
