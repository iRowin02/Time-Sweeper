using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    public float radius;
    public Collider[] enemy;
    public Collider field;

    void Start()
    {
        field = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider coll)
    {
        Freeze(coll);
    }
    void OnTriggerStay(Collider coll)
    {
        Freeze(coll);
    }

    public void Freeze(Collider coll)
    {
        //Collider[] field = 
    }
    void OnTriggerExit(Collider coll)
    {
        UnFreeze();
    }
    void UnFreeze()
    {
        
    }
}
