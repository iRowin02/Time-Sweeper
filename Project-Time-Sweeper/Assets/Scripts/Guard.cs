using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    Guard curTarget;
    Team myTeam;
    Vitals myVitals;

    Transform myTransform;

    Animator anim;

    [SerializeField] float minAttackDamage, maxAttackDamage;
    public enum ai_states
    {
        idle,
        move,
        combat,
    }

    public ai_states states = ai_states.idle;

    void Start()
    {
        myTransform = transform;
        myTeam = GetComponent<Team>();
        myVitals = GetComponent<Vitals>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(myVitals.GetCurrentHealth() > 0)
        {
            switch(states)
            {
                case ai_states.idle:
                    stateIdle();
                    break;
                case ai_states.move:
                    stateMove();
                    break;
                case ai_states.combat:
                    stateCombat();
                    break;
                default:
                    break;
            }
        }
        else
        {
            //DIE
        }
    }

    void stateIdle()
    {

    }

    void stateMove()
    {

    }

    void stateCombat()
    {

    }
}
