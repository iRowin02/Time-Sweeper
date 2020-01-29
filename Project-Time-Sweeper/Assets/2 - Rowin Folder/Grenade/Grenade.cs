using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float radius;
    public float force;
    public float maxDamage;
    private bool hasExploded;
    public GameObject timeFieldObj;
    public enum GrenadeStates
    {
        impact,
        damage,
        time
    }

    public GrenadeStates grenade;
    public GameObject particleEffect;



    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.CompareTag("Floor"))
        {
            if(!hasExploded)
            {
                ExplodeGrenade();   
            }
        }
    }
    void ExplodeGrenade()
    {
        GameObject particle = Instantiate(particleEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearObjects in colliders)
        {
            Rigidbody rb = nearObjects.GetComponent<Rigidbody>();
            
            FakePlayer victimPlayer = nearObjects.gameObject.GetComponent<FakePlayer>();
            if(grenade == GrenadeStates.impact)
            {
                Impact(rb);
            }
            if(grenade == GrenadeStates.damage)
            {

                if(victimPlayer != null)
                {
                    float distance = (transform.position - nearObjects.transform.position).magnitude;

                    victimPlayer.health -= (maxDamage * (1 / distance));

                    Impact(rb);
                }
            }
            if(grenade == GrenadeStates.time)
            {
                GameObject timeField = Instantiate(timeFieldObj, transform.position, Quaternion.identity);
            }
        }

        Object.Destroy(particle, 1);
        Destroy(gameObject);
        hasExploded = true;
    }
    public void Impact(Rigidbody rig)
    {
        if(rig != null)
        {
            rig.AddExplosionForce(force, transform.position, radius);
        }
    }
}
