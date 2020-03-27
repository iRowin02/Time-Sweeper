using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Animator anim;
    private int waitfor = 5;

    private void Awake() 
    {
        anim = GetComponent<Animator>();     
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            print("ok");    
            StartCoroutine(DoorAnim());
        }
    }
    IEnumerator DoorAnim()
    {
        anim.SetBool("NearDoor", true);

        yield return new WaitForSeconds(waitfor);

        anim.SetBool("NearDoor", false);
    }
}
