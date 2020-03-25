using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioClip[] ambientSounds;

    private string sceneName;
    private Scene currentScene;

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

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "StoryScene")
            return;
        StartCoroutine("PlayAmbientSounds");
    }

    public IEnumerator PlayAmbientSounds()
    {
        int index;
        AudioClip toPlay;
        index = Random.Range(0, 2);
        toPlay = ambientSounds[index];
        AudioManager.PlaySound(toPlay, AudioManager.AudioGroups.GameMusic);
        float secondsToWait = toPlay.length;
        print("Ik doe het");
        yield return new WaitForSeconds(secondsToWait);
        StartCoroutine("PlayAmbientSounds");
    }
}
