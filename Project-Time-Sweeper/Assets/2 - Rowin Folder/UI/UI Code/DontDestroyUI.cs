using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    [SerializeField]
    private bool menu = true;
    public GameObject pauseScreen;
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(!menu)
            {
                pauseScreen.SetActive(true);
                if(pauseScreen.activeSelf == true)
                {
                    pauseScreen.SetActive(false);
                }
            }
        }
    }
    public void ResumeButton()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;  
    }
}
