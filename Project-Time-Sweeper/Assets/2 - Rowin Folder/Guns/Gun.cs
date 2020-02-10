using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Gun", order = 0)]
public class Gun : ScriptableObject
{
    public int maxBullets;
    public int currentBullets;
    public float reloadTime;
    public float fireDelay;
    public GameObject bullet;
}
