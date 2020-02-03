using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletspeed;
    public GameObject bulletHole;

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.GetComponent<Collider>())
        {
            Destroy(gameObject);
            Instantiate(bulletHole, gameObject.transform.position, coll.gameObject.transform.rotation);
        }
    }
    void Update()
    {
         transform.position += transform.forward * Time.deltaTime * bulletspeed;
    }
}
