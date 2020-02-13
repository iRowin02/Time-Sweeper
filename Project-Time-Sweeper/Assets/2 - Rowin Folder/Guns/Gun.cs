﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunUsage : MonoBehaviour
{
    [Header("Variables")]
    public float reloadTime;
    public float fireDelay = 0.2f;
    [ReadOnly]public float interval_;
    [ReadOnly]public bool canShoot = true;
    [ReadOnly]public bool isReloading;

    [Header("Usage")]
    public GameObject bullet;
    public Transform barrel;

    [Header("Ammunition")]
    public int currentBullets;
    public int currentAmmo;

    protected int maxAmmo;
}
