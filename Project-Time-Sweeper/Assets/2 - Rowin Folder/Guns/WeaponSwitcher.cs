using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Weapon
{
    public GameObject weaponObject;
    public Image weaponImage;
}

public class WeaponSwitcher : MonoBehaviour
{
    [ReadOnly]public GameObject activeGun;
    [ReadOnly]public int selectedWeapon = 0;
    public Weapon[] weapons;
    public TextMeshProUGUI[] ammo;
    private int ammoDifference;
    

    void Start()
    {
        SelectWeapon();
    }
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedWeapon >= transform.childCount -1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }
        if(ammoDifference != activeGun.GetComponent<FireArms>().currentBullets)
        {
            UpdateAmmo();
        }
        if(Input.GetKey(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if(Input.GetKey(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if(Input.GetKey(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach(Weapon weapon in weapons)
        {
            if(i == selectedWeapon)
            {
                ammoDifference = weapons[i].weaponObject.GetComponent<FireArms>().currentBullets;
                
                weapons[i].weaponObject.SetActive(true);
                weapons[i].weaponImage.GetComponent<CanvasGroup>().alpha = 0.7f;
                activeGun = weapons[i].weaponObject;

                if(activeGun != null)
                {
                    UpdateAmmo();
                }
            }
            else
            {
                weapons[i].weaponObject.SetActive(false);
                weapons[i].weaponImage.GetComponent<CanvasGroup>().alpha = 0.2f;
            }
            i++;
        }
    }
    public void UpdateAmmo()
    {
       ammo[0].text = activeGun.GetComponent<FireArms>().currentBullets.ToString();
       ammo[1].text = activeGun.GetComponent<FireArms>().currentAmmo.ToString();
    }
}
