using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Jobs;
using UnityEngine.UI;
using TMPro;

public class ReadOnlyAttribute : PropertyAttribute { }

public class PlayerInfo : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private HUD_Manager HUD;
    [Header("Variables")]
    public float maxHealth;
    [ReadOnly]
    public float playerHealth;

    public int playerMana;
    private int _playerMana;

    public event Action<float> OnHealthPctChange = delegate { };

    [SerializeField]
    private float manaRate;

    private void Start()
    {
        playerHealth = maxHealth;
    }

    #region Health

    public void HealthUpdate(int damage)
    {
        playerHealth += damage;

        float currentHealthPct = playerHealth / maxHealth;

        OnHealthPctChange(currentHealthPct);
        if(playerHealth > 100)
        {
            playerHealth = maxHealth;
        }
    }
    
    #endregion

    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            HealthUpdate(-10);
        }
    }
}
