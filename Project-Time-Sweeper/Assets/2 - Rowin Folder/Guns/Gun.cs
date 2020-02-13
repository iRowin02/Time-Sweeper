using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUsage : MonoBehaviour
{
    [Header("Variables")]
    public float reloadTime;
    public float fireDelay = 0.2f;
    [ReadOnly]public float interval_;
    [ReadOnly]public bool canShoot = true;

    [Header("Usage")]
    public GameObject bullet;
    public Transform barrel;
    public static Gun gunInfo;
}
[System.Serializable]
[CreateAssetMenu(fileName = "Gun", menuName = "Weapons/Guns", order = 0)]
public class Gun : ScriptableObject
{
    [Header("Ammunition")]
    public int currentAmmo;
    public int currentBullets;
    //private int 
}
