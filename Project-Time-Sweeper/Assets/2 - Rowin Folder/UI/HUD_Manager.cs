using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Weapons
{
    public Image weaponImage;
    public int maxAmmo, currentAmo;
}

public class HUD_Manager : MonoBehaviour
{
    [Header("Managers")]
    public PlayerMove playerInfo;
    [Header("ManaBar Elements")]
    public Image manaBar;
    public TextMeshProUGUI manaTextAmount;
    private float manaValue = .20f;
    public float totalMana;
    private float totalMana_;
    [Header("Pause Elements")]
    public GameObject pauseScreen;
    public bool isPaused;
    [Header("Other Elements")]
    public GameObject selfDestruct;
    public TextMeshProUGUI selfDestructText;
    [Header("Variables")]
    [SerializeField]
    private float manaUpdateSeconds = 0.02f;
    public void Update()
    {
        if (totalMana_ != totalMana)
        {
            totalMana_ = totalMana;
            manaTextAmount.text = "(" + totalMana.ToString() + ")";
            StartCoroutine(HandleManaChange((int)totalMana));
        }
        InputUpdate();
    }
    void InputUpdate()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
        }

        if(isPaused)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        if(!isPaused)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Awake()
    {
        manaTextAmount.text = "(" + totalMana.ToString() + ")";
    }

    #region HandleMana

    public IEnumerator HandleManaChange(int manaAmount)
    {
        //int maxMana = 5;
        //int minMana = 0;

        float preMana = manaBar.fillAmount;
        float mana = manaValue * manaAmount;

        float timeElapsed = 0f;

        while (timeElapsed < manaUpdateSeconds)
        {
            timeElapsed += Time.deltaTime;
            manaBar.fillAmount = Mathf.Lerp(preMana, mana, timeElapsed / manaUpdateSeconds);

            yield return null;
        }
        manaBar.fillAmount = mana;
    }

    #endregion

    #region SelfDestruct
    public void SelfDestructEngage(float time)
    {
        selfDestruct.SetActive(true);

        StartCoroutine(SelfDestructTimer(time));

    }
    IEnumerator SelfDestructTimer(float timeLeft)
    {
        while(true)
        {
            timeLeft -= 1;

            int min = Mathf.FloorToInt(timeLeft / 60);
            int sec = Mathf.FloorToInt(timeLeft % 60);

            UpdateDestructText(min, sec);

            yield return new WaitForSeconds(1);
            
            if(timeLeft <= 0)
            {
                StopCoroutine(SelfDestructTimer(timeLeft));
            }
        }
    }
    void UpdateDestructText(int min, int sec)
    {
        selfDestructText.text = min.ToString("00") + ":" + sec.ToString("00");
    }
    #endregion
    #region Buttons
    public void ResumeButton()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}

