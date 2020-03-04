using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoPickUp : MonoBehaviour
{
    public int minAmmo, maxAmmo;
    [ReadOnly] public int ammoAmount;
    public TextMeshProUGUI ammoAmountDisplay;
    
    void Start()
    {
        ammoAmount = Random.Range(minAmmo, maxAmmo);
        ammoAmountDisplay.text = ammoAmount.ToString();
    }
    void OnTCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.GetComponentInChildren<WeaponSwitcher>().activeGun.gameObject.GetComponent<FireArms>().currentAmmo += ammoAmount;
            coll.gameObject.GetComponentInChildren<WeaponSwitcher>().UpdateAmmo();
            Destroy(gameObject);
        }
    }

}
