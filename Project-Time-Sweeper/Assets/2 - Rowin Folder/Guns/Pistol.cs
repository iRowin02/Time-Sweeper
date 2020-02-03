using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Transform barrel;
    public GameObject bullet;
    private bool canShoot = true;
    public float interval;
    private float interval_;

    void Start()
    {
        interval_ = interval;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(canShoot)
            {
                Instantiate(bullet, barrel.position, Quaternion.identity);
                canShoot = false;
            }
        }

        if(!canShoot)
        {
            interval -= Time.deltaTime;


            if(interval <= 0)
            {
                interval = interval_;
                canShoot = true;
            }
        }
    }
}

