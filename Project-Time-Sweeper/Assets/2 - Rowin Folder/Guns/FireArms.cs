using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArms : GunUsage
{
    void Start()
    {
        interval_ = fireDelay;
        maxAmmo = currentBullets;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //if(gunInfo.currentBullets <= 0)
            //{
                //StartCoroutine(Reload());
                //return;
            //}
            Shoot();
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
    }

    public void Shoot()
    {
        if(currentBullets > 1 && isReloading != true)
        {
            if (canShoot && !isReloading)
            {
                Instantiate(bullet, barrel.position, transform.rotation);
                currentBullets--;

                canShoot = false;
            }
            return;
        }
        else
        {
            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;

        currentBullets = maxAmmo;
        currentAmmo -= maxAmmo;

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }
}

