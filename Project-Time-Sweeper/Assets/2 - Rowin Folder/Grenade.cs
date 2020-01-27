using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float timer;
    public float radius;
    public float force;
    private float countdown;
    private bool hasExploded;
    public GameObject particleEffect;


    void Start()
    {
        countdown = timer;
    }
    void Update()
    {
        TimerGrenade();
    }
    void TimerGrenade()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0 && !hasExploded)
        {
            ExplodeGrenade();
        }
    }
    void ExplodeGrenade()
    {
        GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearObjects in colliders)
        {
            Rigidbody rb = nearObjects.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Object.Destroy(particle, 1);
        Destroy(gameObject);
        hasExploded = true;
    }
}
