using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject loadingScreen, settingsScreen, mainMenu;
    public Animator transition;
    public int loadingTime;
    public Slider progressSlider;
    public TextMeshProUGUI loadText;

    public void StartButton(int level)
    {
        LevelLoading(level);
    }
    void LevelLoading(int level)
    {
        transition.SetTrigger("Play");

        StartCoroutine(LoadLevel(level));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SettingsButton()
    {
        settingsScreen.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void BackButton()
    {
        settingsScreen.SetActive(false);
        mainMenu.SetActive(true);
    }
    IEnumerator LoadLevel(int sceneIndex)
    {
        yield return new WaitForSeconds(loadingTime);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            progressSlider.value = progress;

            loadText.text = "Loading: " + progress * 100f +"%";
            
            yield return null;
        }
    }
}
