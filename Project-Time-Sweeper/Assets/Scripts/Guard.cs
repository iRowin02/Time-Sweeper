using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public Player curTarget;
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

    Vector3 targetLastKnownPos;
    Path currentPath = null;

    public AudioClip fire;

    public enum ai_states
    {
        idle,
        move,
        combat,
        Investigate,
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
        if (myVitals.GetCurrentHealth() > 0)
        {
            switch (states)
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
                case ai_states.Investigate:
                    StateInvestigate();
                    break;
                default:
                    break;
            }
        }
        else
        {
            //DIE
            anim.SetBool("move", false);

            if(GetComponent<BoxCollider>() != null)
            {
                Destroy(GetComponent<BoxCollider>());
            }

            Quaternion deathRot = Quaternion.Euler(-90, myTransform.rotation.eulerAngles.y, myTransform.rotation.eulerAngles.z);
            if(myTransform.rotation != deathRot)
            {
                myTransform.rotation = deathRot;
            }
        }
    }

    void stateIdle()
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
                anim.SetBool("move", true);
                states = ai_states.move;
            }
        }
        else
        {
            //FIND TARGET
            Player[] allPlayers = GameObject.FindObjectsOfType<Player>(); 
            Player bestTarget = null;

            for (int i = 0; i < allPlayers.Length; i++)
            {
                Player curPlayer = allPlayers[i];

                if (curPlayer.GetComponent<Team>().getTeamNumber() != myTeam.getTeamNumber() && curPlayer.GetComponent<Vitals>().GetCurrentHealth() > 0)
                {
                    if (canISeeTarget(curPlayer.transform))
                    {
                            print("Searching");
                        if (bestTarget == null)
                        {
                            bestTarget = curPlayer;
                        }
                        else
                        {
                            //CHANGE IF BETTER IS FOUND
                            if (Vector3.Distance(curPlayer.transform.position, myTransform.position) < Vector3.Distance(bestTarget.transform.position, myTransform.position))
                            {
                                bestTarget = curPlayer;
                            }
                        }
                    }
                }
            }
            if (bestTarget != null)
            {
                curTarget = bestTarget;
            }

        }
    }

    void stateMove()
    {
        if (curTarget != null && curTarget.GetComponent<Vitals>().GetCurrentHealth() > 0)
        {
            if (Vector3.Distance(myTransform.position, curTarget.transform.position) > maxAttackDst)
            {
                //MOVE CLOSER
                myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if (Vector3.Distance(myTransform.position, curTarget.transform.position) < minAttackDst)
            {
                //MOVE AWAY
                myTransform.Translate(Vector3.forward * -1 * moveSpeed * Time.deltaTime);
            }
            else
            {
                //ATTACK
                anim.SetBool("move", false);
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
            if (!canISeeTarget(curTarget.transform))
            {
                targetLastKnownPos = curTarget.transform.position;

                currentPath = CalcPath(myTransform.position, targetLastKnownPos);

                anim.SetBool("move", true);
                states = ai_states.Investigate;
                return;
            }

            myTransform.LookAt(curTarget.transform);
            if (Vector3.Distance(myTransform.position, curTarget.transform.position) <= maxAttackDst && Vector3.Distance(myTransform.position, curTarget.transform.position) >= minAttackDst)
            {
                //ATTACK
                if (curFireRate <= 0)
                {
                    anim.SetTrigger("fire");
                    AudioManager.PlaySound(fire, AudioManager.AudioGroups.GameSFX);

                    float outCome = Random.Range(0, 2);
                    if(outCome == 1)
                    {
                        curTarget.GetComponent<Vitals>().getHit(damage);
                    }

                    curFireRate = fireRate;
                }
                else
                {
                    curFireRate -= 1 * Time.deltaTime;
                }
            }
            else
            {
                //MOVE
                anim.SetBool("move", true);
                states = ai_states.move;
            }
        }
        else
        {
            if(curTarget != null && curTarget.GetComponent<Vitals>().GetCurrentHealth() <= 0)
            {
                targetLastKnownPos = curTarget.transform.position;

                currentPath = CalcPath(myTransform.position, targetLastKnownPos);

                anim.SetBool("move", true);
                states = ai_states.Investigate;
            }
            else
            {
                states = ai_states.idle;
            }
        }
    }

    void StateInvestigate()
    {
        if(currentPath != null)
        {
            if(currentPath.ReachedEndNode())
            {
                anim.SetBool("move", false);

                currentPath = null;
                curTarget = null;

                states = ai_states.idle;
                return;
            }

            Vector3 nodePos = currentPath.GetNextNode();

            if (Vector3.Distance(myTransform.position, nodePos) < 1)
            {
                currentPath.currentPathIndex++;
            }
            else
            {
                myTransform.LookAt(nodePos);
                myTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
        else
        {

        }
    }

    bool canISeeTarget(Transform target)
    {
        bool canSeeIt = false;
        //VISABLE?
        Vector3 myPos = myTransform.position;
        myPos.y = myTransform.position.y + 0.5f; //CAST UIT ZIJN MIDDEL

        Vector3 playerPos = target.position;
        playerPos.y = target.position.y + 0.5f; //NAAR MIDDEL VAN

        Vector3 dirToPlayer = playerPos - myPos;

        RaycastHit hit;
        if (Physics.Raycast(myPos, dirToPlayer, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(myPos, dirToPlayer, Color.green);
            if (hit.transform == target)
            {
                canSeeIt = true;
            }
        }

        return canSeeIt;
    }

    Path CalcPath(Vector3 source, Vector3 destination)
    {
        NavMeshPath nvPath = new NavMeshPath();
        NavMesh.CalculatePath(source, destination, NavMesh.AllAreas, nvPath);

        Path path = new Path(nvPath.corners);

        return path;
    }
}