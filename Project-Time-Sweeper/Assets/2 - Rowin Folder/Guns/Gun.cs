using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun", order = 1)]
public class GunInfo : ScriptableObject
{
    public int maxBullets;
    public int currentBullets;
}
public class Gun : MonoBehaviour
{
    public float reloadTime;
    public float fireDelay;
    public GameObject bullet;
}
