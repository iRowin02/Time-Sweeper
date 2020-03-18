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

    [Header("General")]
    public float moveSpeed;
    public float damage;

    [Header("Sight")]
    public float minAttackDst, maxAttackDst;


    public float fireRate;
    private float curFireRate = 0;

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
            Destroy(this.gameObject);
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
                anim.SetBool("move", true);
                //MOVE
                states = ai_states.move;
            }
        }
        else
        {
            //FIND TARGET
            Guard[] allGuards = GameObject.FindObjectsOfType<Guard>();
            Guard bestTarget = null;

            for (int i = 0; i < allGuards.Length; i++)
            {
                Guard curGuard = allGuards[i];

                //VISABLE?
                Vector3 myPos = myTransform.position;
                myPos.y = myTransform.position.y + 0.5f; //CAST UIT ZIJN MIDDEL

                Vector3 playerPos = curGuard.transform.position;
                playerPos.y = curGuard.transform.position.y + 0.5f; //NAAR MIDDEL VAN

                Vector3 dirToPlayer = playerPos - myPos;

                RaycastHit hit;
                if(Physics.Raycast(myPos, dirToPlayer, out hit, Mathf.Infinity))
                {
                    //if(hit.)
                }

                if (curGuard.GetComponent<Team>().getTeamNumber() != myTeam.getTeamNumber() && curGuard.GetComponent<Vitals>().GetCurrentHealth() > 0)
                {
                    if(bestTarget == null)
                    {
                        bestTarget = curGuard;
                    }
                    else
                    {
                        //CHANGE IF BETTER IS FOUND
                        if(Vector3.Distance(curGuard.transform.position, myTransform.position) < Vector3.Distance(bestTarget.transform.position, myTransform.position))
                        {
                            bestTarget = curGuard;
                        }
                    }
                }
            }
            if(bestTarget != null)
            {
                curTarget = bestTarget;       
            }
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
                anim.SetBool("move", false);
                //ATTACK
                states = ai_states.combat;
            }
        }
        else
        {
            anim.SetBool("move", false);
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
                if(curFireRate <= 0)
                {
                    anim.SetTrigger("fire");

                    curTarget.GetComponent<Vitals>().getHit(damage);

                    curFireRate = fireRate;
                }
                else
                {
                    curFireRate -= 1 * Time.deltaTime;
                }
            }
            else
            {
                anim.SetBool("move", true);
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
