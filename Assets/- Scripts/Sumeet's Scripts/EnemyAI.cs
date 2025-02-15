using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> destinations;
    public Animator anim;
    public float walkSpeed, chaseSpeed, idleTime;
    public int minIdleTime, maxIdleTime;
    public bool walking, chasing;
    public int destinationAmount;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    int randnum, randnum2;
    
    void Start()
    {
        walking = true;
        randnum = Random.Range(0,destinationAmount);
        currentDest= destinations[randnum];
    }

    // Update is called once per frame
    void Update()
    {
        if(walking==true)
        {
            dest = currentDest.position;
            agent.destination=dest;
            agent.speed= walkSpeed;

            if(agent.remainingDistance <=agent.stoppingDistance)
            {
                randnum2=Random.Range(0,2);
                if(randnum2==0)
                {
                    randnum=Random.Range(0,destinationAmount);
                    currentDest=destinations[randnum];
                }

                if(randnum2==1)
                {
                    anim.ResetTrigger("walk");
                    anim.SetTrigger("idle");
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());

                }
            }
        }
        
    }

    IEnumerator StayIdle()
    {
        idleTime = Random.Range(minIdleTime,maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randnum= Random.Range(0,destinationAmount);
        currentDest= destinations[randnum];
        anim.ResetTrigger("idle");
        anim.SetTrigger("walk");

    }
}
