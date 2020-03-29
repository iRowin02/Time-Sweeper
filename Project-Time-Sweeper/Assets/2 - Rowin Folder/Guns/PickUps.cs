using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUps : MonoBehaviour
{  
    public int min, max;
    public bool health, ammo;
    [ReadOnly] public int amount;
    public TextMeshProUGUI amountDisplay;
    
    void Awake()
    {
        amount = Random.Range(min, max);
        amountDisplay.text = amount.ToString();
    }
    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            if(health == true)
            {
                var obj = coll.gameObject.GetComponent<Vitals>().GetCurrentHealth();
                obj += amount;
            }
            if(ammo == true)
            {
                coll.gameObject.GetComponentInChildren<WeaponSwitcher>().activeGun.gameObject.GetComponent<FireArms>().currentAmmo += amount;
                coll.gameObject.GetComponentInChildren<WeaponSwitcher>().UpdateAmmo();
            }
            Destroy(gameObject);
        }
    }
}
