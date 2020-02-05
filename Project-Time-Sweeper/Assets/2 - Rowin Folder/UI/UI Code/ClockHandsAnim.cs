using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClockHandsAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator clock1;
    public Animator clock2;

    public void OnPointerEnter(PointerEventData evendata)
    {
        clock1.speed = 100f;
        clock2.speed = 100f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        clock1.speed = 1f;
        clock2.speed = 1f;
    }

}
