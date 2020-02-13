using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHighlighter : MonoBehaviour
{
    void Start()
    {
        foreach(GameObject weaponImage in transform)
        {
            print(weaponImage.name);
        }
    }
}
