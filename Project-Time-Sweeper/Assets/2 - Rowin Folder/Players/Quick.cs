using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quick : MonoBehaviour
{
   public List<string> speechBubbleText = new List<string>();
   public GameObject speechBubble;
   public TextMeshProUGUI speechText;
   public int textLine;

   private void OnTriggerEnter(Collider other) 
   {
        if(other.gameObject.CompareTag("Player"))
        {
            speechBubble.SetActive(true);
            SetText();
        }
   }
   private void OnTriggerExit(Collider other) 
   {
       if(other.gameObject.CompareTag("Player"))
        {
            speechBubble.SetActive(false);
        }
   }
   private void Update() 
   {
       if(Input.GetKeyDown(KeyCode.P))
       {
           textLine++;
           SetText();
       }
   }
   void SetText()
   {
        speechText.text = speechBubbleText[textLine];  
   }
}
