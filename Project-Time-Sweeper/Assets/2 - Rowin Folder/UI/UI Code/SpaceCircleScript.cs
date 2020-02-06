using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpaceCircleScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator animator;
    public Animator[] clockHands;
    public StartScreenCode startScreen;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("PlayAnim", true);

        foreach (Animator anim in clockHands)
        {
            anim.speed = 50f;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("PlayAnim", false);

        foreach (Animator anim in clockHands)
        {
            anim.speed = 1f;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        startScreen.Disable();
    }
}
