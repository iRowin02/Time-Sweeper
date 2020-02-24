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
    public PlayerInfo playerInfo;
    [Header("UI Elements")]
    public Image healthBar;
    [Header("Variables")]

    [SerializeField]
    private float updateSeconds = 0.2f;

    public void Awake()
    {
        healthBar.fillAmount = 1;
        playerInfo.GetComponent<PlayerInfo>().OnHealthPctChange += HandleHealthChange;
    }

    #region HandleHealth

    private void HandleHealthChange(float pct)
    {
        StartCoroutine(ChangePct(pct));
        print("ok");
    }

    private IEnumerator ChangePct(float pct)
    {
        float preChangePct = healthBar.fillAmount;
        float elapsed = 0f;

        while(elapsed < updateSeconds)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSeconds);

            yield return null;
        }
        healthBar.fillAmount = pct;
    }
    #endregion
}

