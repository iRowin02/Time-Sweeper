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
    public int selectedWeapon = 0;
    public Weapon[] weapons;
    public TextMeshProUGUI[] ammo;

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
                weapons[i].weaponObject.SetActive(true);
                weapons[i].weaponImage.GetComponent<CanvasGroup>().alpha = 0.7f;
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
        
    }
}
