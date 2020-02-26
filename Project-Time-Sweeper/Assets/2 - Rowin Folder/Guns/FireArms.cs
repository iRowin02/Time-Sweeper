using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArms : GunUsage
{
    private WeaponSwitcher weaponSwitcher;

    public int leftOver;

    void Start()
    {
        weaponSwitcher = GetComponentInParent<WeaponSwitcher>();
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
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
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
        // if(Input.GetButtonDown("R"))
        // {
        //     leftOver = maxAmmo -= currentBullets;

        //     if(leftOver > 0)
        //     {
        //         StartCoroutine(Reload(leftOver));
        //     }
        //     if(leftOver < 0)
        //     {
        //         leftOver = 0;
        //         return;
        //     }
        // }
    }

    public void Shoot()
    {
        if (canShoot && !isReloading)
        {
            RaycastHit hit;
            //Physics.IgnoreLayerCollision(1,11);
            Vector3 ray = cam.transform.position;

            if (Physics.Raycast (ray, cam.transform.forward, out hit,float.PositiveInfinity, layer))
            {
                print(hit.collider.gameObject.name);
                Debug.DrawRay(ray, hit.point, Color.red, 15f);
            }
            //print(hit.transform.gameObject.name);

            Instantiate(bullet, barrel.position, transform.rotation);
            currentBullets--;

            canShoot = false;
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
}

