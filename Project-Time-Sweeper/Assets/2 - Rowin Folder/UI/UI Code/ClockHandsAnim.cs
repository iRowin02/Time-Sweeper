using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClockHandsAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator[] clockHands;

    public void OnPointerEnter(PointerEventData evendata)
    {
        foreach (Animator anim in clockHands)
        {
            anim.speed = 100f;    
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Animator anim in clockHands)
        {
            anim.speed = 1f;    
        }
    }

}
