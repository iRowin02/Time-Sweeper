using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitals : MonoBehaviour
{
    [SerializeField] float health;
    float curHealth;

    void Start()
    {
        curHealth = health;
    }

    public float GetCurrentHealth()
    {
        return curHealth;
    }

    public void getHit(float damage)
    {
        curHealth -= damage;
    }
}
