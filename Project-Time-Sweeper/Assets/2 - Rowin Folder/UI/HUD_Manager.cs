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
    public ThirdPersonMovement.ThirdPersonController playerInfo;
    [Header("HealthBar Elements")]
    public Image healthBar;
    public TextMeshProUGUI healthAmount;
    [Header("ManaBar Elements")]
    public Image[] manaBar;
    public int manaAmount;
    [Header("Variables")]
    [SerializeField]
    private float updateSeconds = 0.2f;

    public void Awake()
    {
        healthAmount.text = playerInfo.playerHealth.ToString();
        healthBar.fillAmount = 1;
        playerInfo.OnHealthPctChange += HandleHealthChange;
    }

    #region HandleHealth

    private void HandleHealthChange(float pct)
    {
        StartCoroutine(ChangePct(pct));
    }

    private IEnumerator ChangePct(float pct)
    {
        float preChangePct = healthBar.fillAmount;
        float elapsed = 0f;

        while(elapsed < updateSeconds)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSeconds);
            healthAmount.text = playerInfo.playerHealth.ToString();

            yield return null;
        }
        healthBar.fillAmount = pct;
    }
    #endregion
    #region HandleMana

    public void HandleManaChange()
    {
        //for(int i = 0; i < manaBar.Length; i++)
        //{
//
        //}
    }

    #endregion
}

