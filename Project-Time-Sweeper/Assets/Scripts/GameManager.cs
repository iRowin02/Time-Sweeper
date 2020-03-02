using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() 
    {
        //Niks hiervoor!!!!!
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public void Start() 
    {
        AudioManager.audioMixer = Resources.Load("MainMixer") as AudioMixer;   
    }
}
