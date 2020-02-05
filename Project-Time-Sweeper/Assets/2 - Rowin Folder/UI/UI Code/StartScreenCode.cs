using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartScreenCode : MonoBehaviour
{   
    public GameObject mainMenu, transition;
    public GameObject startScreen;

    void Start()
    {
        startScreen = gameObject;
    }

    public void Disable()
    {
        startScreen.SetActive(false);
        transition.SetActive(true);
        mainMenu.SetActive(true);
    }
}
