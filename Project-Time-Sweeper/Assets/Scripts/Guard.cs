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

    public float minAttackDst, maxAttackDst;
    public float moveSpeed;

    public float fireRate;

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
        if(curTarget != null && curTarget.GetComponent<Vitals>().GetCurrentHealth() > 0)
        {
            myTransform.LookAt(curTarget.transform);
            if (Vector3.Distance(myTransform.position, curTarget.transform.position) <= maxAttackDst && Vector3.Distance(myTransform.position, curTarget.transform.position) >= minAttackDst)
            {
                //ATTACK
                states = ai_states.combat;
            }
            else
            {
                //MOVE
                states = ai_states.move;
            }
        }
        else
        {
            //FIND TARGET
        }
    }

    void stateMove()
    {
        if (curTarget != null && curTarget.GetComponent<Vitals>().GetCurrentHealth() > 0)
        {
            if(Vector3.Distance(myTransform.position, curTarget.transform.position) > maxAttackDst)
            {
                //MOVE CLOSER
                myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if(Vector3.Distance(myTransform.position, curTarget.transform.position) < minAttackDst)
            {
                //MOVE AWAY
                myTransform.Translate(Vector3.forward * -1 * moveSpeed * Time.deltaTime);
            }
            else
            {
                //ATTACK
                states = ai_states.combat;
            }
        }
        else
        {
            states = ai_states.idle;
        }
    }

    void stateCombat()
    {
        if (curTarget != null && curTarget.GetComponent<Vitals>().GetCurrentHealth() > 0)
        {    
            myTransform.LookAt(curTarget.transform);
            if (Vector3.Distance(myTransform.position, curTarget.transform.position) <= maxAttackDst && Vector3.Distance(myTransform.position, curTarget.transform.position) >= minAttackDst)
            {
                //ATTACK
                states = ai_states.combat;
            }
            else
            {
                //MOVE
                states = ai_states.move;
            }
        }
        else
        {
            states = ai_states.idle;
        }
    }
}
