using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    public float health;
    private float speed;
    protected float standardSpeed;

    public states enemyStates;

    public enum states
    {
        Walking,
        TimeFrozen
    }

    void Start()
    {
        standardSpeed = speed;
    }
    void Update()
    {
        if(enemyStates == states.Walking)
        {
            speed = standardSpeed;
        }
        if(enemyStates == states.TimeFrozen)
        {
            speed = standardSpeed;
        }
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        transform.position -= new Vector3(speed, 0f, 0f) * Time.deltaTime;
    }
}
