using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEngage : MonoBehaviour
{
    [SerializeField]
    private HUD_Manager hud;
    [SerializeField]
    private float time;

    public AudioClip alarm;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            hud.SelfDestructEngage(time);

        }
    }
}
