using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpaceCircleScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public Animator animator;
    public Animator clock1;
    public Animator clock2;
    public StartScreenCode startScreen;

    public Image image;
    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("PlayAnim", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("PlayAnim", false);

        clock1.speed = 1f;
        clock2.speed = 1f;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        startScreen.Disable();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        clock1.speed = 70f;
        clock2.speed = 70f;
    }
}
