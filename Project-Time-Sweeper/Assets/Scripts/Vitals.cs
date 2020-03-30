using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitals : MonoBehaviour
{
    [SerializeField] float health;
    float curHealth;
    private PlayerMove player;

    void Start()
    {
        curHealth = health;
        player = GetComponent<PlayerMove>();
    }

    public float GetCurrentHealth()
    {
        return curHealth;
    }

    public void getHit(float damage)
    {
        curHealth -= damage;
        player.HealthUpdate();
    }
}
