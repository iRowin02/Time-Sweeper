using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public GunInfo gunInfo;
    public Transform barrel;
    public bool canShoot = true;
    public float interval_;

    void Start()
    {
        interval_ = fireDelay;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
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
}

