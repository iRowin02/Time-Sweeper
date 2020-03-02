using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager 
{
    public static AudioMixer audioMixer; // this isnt right geen instance

    public enum AudioGroups
    {
        None,
        UIMusic,
        GameMusic,
        GameSFX,
        UISFX,
    }
    public static void PlaySound(AudioClip audioClipToPlay, AudioGroups audioGroups)
    {
        GameObject soundGameobject = new GameObject("Sound");
        AudioSource audioSource = soundGameobject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(audioClipToPlay);
        soundGameobject.GetComponent<AudioSource>().outputAudioMixerGroup = AudioManager.audioMixer.FindMatchingGroups(audioGroups.ToString())[0];
        GameObject.Destroy(soundGameobject, audioClipToPlay.length);
    }
}
//AudioManager.PlaySound(CLIP!, AudioManager.AudioGroups.AUDIOGROEP!);
//AudioManager.audioMixer = Resources.Load("MasterVolume") as AudioMixer;