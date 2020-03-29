using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEngage : MonoBehaviour
{
    [SerializeField]
    private HUD_Manager hud;
    [SerializeField]
    private float time;
    public AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            hud.SelfDestructEngage(time);
            AudioManager.PlaySound(clip, AudioManager.AudioGroups.GameMusic);
            Destroy(gameObject);
        }
    }
}
