using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArms : GunUsage
{
    void Start()
    {
        interval_ = fireDelay;
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
        if (canShoot)
        {
            Instantiate(bullet, barrel.position, transform.rotation);
            canShoot = false;
        }
    }
    //IEnumerator Reload()
    //{
    //    
    //}
}

