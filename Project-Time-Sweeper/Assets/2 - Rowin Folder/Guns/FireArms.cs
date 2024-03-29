﻿using System.Collections;
using UnityEngine;

public class FireArms : GunUsage
{
    private WeaponSwitcher weaponSwitcher;

    public int leftOver;

    void Start()
    {
        weaponSwitcher = GetComponentInParent<WeaponSwitcher>();
        HUD = FindObjectOfType<HUD_Manager>();
        interval_ = fireDelay;
        maxAmmo = currentBullets;
        maxBullets = currentAmmo;

        cam = Camera.main;
        layer = ~layer;
    }

    void Update()
    {
        if(currentAmmo > maxBullets)
        {
            currentAmmo = maxBullets;
            weaponSwitcher.UpdateAmmo();
        }
        if(currentBullets <= 0)
        {
            StartCoroutine(Reload(maxAmmo));
            return;
        }

        if(!isReloading)
        {
            if (Input.GetButton("Fire1"))
            {
                if(HUD.isPaused == false)
                {
                    if(currentAmmo >= 0)
                    {
                        Shoot();
                    }
                }
            }
        }
        
        if(!canShoot)
        {
            fireDelay -= Time.deltaTime;

            if(fireDelay <= 0)
            {
                fireDelay = interval_;
                canShoot = true;
            }
        }
        if(Input.GetButtonDown("R"))
        {
            leftOver = maxAmmo -= currentBullets;

            if(leftOver > 0)
             {
                 StartCoroutine(Reload(leftOver));
             }
             if(leftOver < 0)
             {
                 leftOver = 0;
                 return;
             }
        }
    }

    public void Shoot()
    {
        if (canShoot && !isReloading)
        {
            RaycastHit hit;
            Vector3 ray = cam.transform.position;

            if (Physics.Raycast (ray, cam.transform.forward, out hit,float.PositiveInfinity, layer))
            {
                Fire(hit);
            }

            GameObject muzzle = Instantiate(muzzleFlash, barrel.position, transform.rotation);
            canShoot = false;
            Destroy(muzzle, 0.2f);
        }
    }
    IEnumerator Reload(int ammoLeft)
    {
        isReloading = true;

        currentBullets += ammoLeft;
        currentAmmo -= ammoLeft;

        yield return new WaitForSeconds(reloadTime);

        weaponSwitcher.UpdateAmmo();
        isReloading = false;
    }
    void Fire(RaycastHit hit)
    {
        print(hit.collider.gameObject.name);
        AudioManager.PlaySound(gunSound, AudioManager.AudioGroups.GameSFX);
        float damage = Random.Range(15, 20);
        if(hit.collider.gameObject.GetComponent<Guard>())
        {
            hit.collider.gameObject.GetComponent<Vitals>().getHit(damage);
        }
        if(hit.collider.gameObject.GetComponent<ExplosiveBarrels>())
        {
            hit.collider.gameObject.GetComponent<ExplosiveBarrels>().Explossion();
        }
        currentBullets--;
    }
}

