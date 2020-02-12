using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Gun pistolInfo;
    public Transform barrel;
    private bool canShoot = true;
    private float interval_;

    void Start()
    {
        interval_ = pistolInfo.fireDelay;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            Instantiate(pistolInfo.bullet, barrel.position, transform.rotation);
            canShoot = false;
        }
        if(!canShoot)
        {
            pistolInfo.fireDelay-= Time.deltaTime;


            if(pistolInfo.fireDelay <= 0)
            {
                pistolInfo.fireDelay = interval_;
                canShoot = true;
            }
        }
    }
}

