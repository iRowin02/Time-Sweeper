using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletspeed;

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
         transform.position += transform.forward * Time.deltaTime * bulletspeed;
    }
}
