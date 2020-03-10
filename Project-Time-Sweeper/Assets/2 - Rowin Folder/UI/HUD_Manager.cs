using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("HealthBar Elements")]
    public Image healthBar;
    public TextMeshProUGUI healthAmount;
    [Header("ManaBar Elements")]
    public Image manaBar;
    public TextMeshProUGUI manaTextAmount;
    private float manaValue = .20f;
    public float totalMana;
    private float totalMana_;
    [Header("Variables")]
    [SerializeField]
    private float healthUpdateSeconds = 0.2f;
    [SerializeField]
    private float manaUpdateSeconds = 0.02f;

    public void Update()
    {
        if(totalMana_ != totalMana)
        {
            totalMana_ = totalMana;
            manaTextAmount.text = "(" + totalMana.ToString() + ")";
            StartCoroutine(HandleManaChange((int)totalMana));
        }
    }

    public void Awake()
    {
        manaTextAmount.text = "(" + totalMana.ToString() + ")";
        healthAmount.text = "(" + playerInfo.playerHealth.ToString() + ")";
        healthBar.fillAmount = 1;
        playerInfo.OnHealthPctChange += HandleHealthChange;
    }

    #region HandleHealth

    public void HandleHealthChange(float pct)
    {
        StartCoroutine(ChangePct(pct));
    }

    private IEnumerator ChangePct(float pct)
    {
        float preChangePct = healthBar.fillAmount;
        float elapsed = 0f;

        while(elapsed < healthUpdateSeconds)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / healthUpdateSeconds);
            healthAmount.text = "(" + playerInfo.playerHealth.ToString() + ")";

            yield return null;
        }
        healthBar.fillAmount = pct;
    }
    #endregion

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
}

