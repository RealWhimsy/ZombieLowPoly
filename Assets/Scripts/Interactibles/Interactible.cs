using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManagerRework.Instance.PlayEffectOneShot(Resources.Load(Const.SFX.AmmoPickup) as AudioClip);
            EventManager.TriggerEvent(Const.Events.InteractibleCollected);
            Destroy(gameObject);
        }
    }
}
