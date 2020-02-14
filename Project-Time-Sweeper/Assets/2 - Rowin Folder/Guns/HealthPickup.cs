using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPickup : MonoBehaviour
{  
    public int minHealth, maxHealth;
    [ReadOnly] public int healthAmount;
    public TextMeshProUGUI healthAmountDisplay;
    
    void Start()
    {
        healthAmount = Random.Range(minHealth, maxHealth);
        healthAmountDisplay.text = healthAmount.ToString();
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<PlayerInfo>().HealthUpdate(healthAmount);
            Destroy(gameObject);
        }
    }
}
