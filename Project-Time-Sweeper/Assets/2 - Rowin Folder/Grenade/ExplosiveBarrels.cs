using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour
{
    public GameObject explossion;
    public GameObject exploded;
    
    public void Explossion()
    {
        Destroy(gameObject);

        GameObject particle = Instantiate(explossion, transform.position, Quaternion.identity);
        
        Instantiate(exploded, transform.position, Quaternion.identity);

        Destroy(particle);
    }

}
