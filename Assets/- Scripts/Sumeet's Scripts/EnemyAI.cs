using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> destinations;
    public Animator anim;
    public float walkSpeed, chaseSpeed, idleTime, sightDistance,
    catchDistance, chaseTime, minchaseTime, maxchaseTime, jumpScareTime;
    public int minIdleTime, maxIdleTime;
    public bool walking, chasing;
    public int destinationAmount;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    int randnum, randnum2;
    public Vector3 raycastOffset;
    Vector3 direction;
    //public string deathScene;


    void Start()
    {
        walking = true;
        randnum = Random.Range(0, destinationAmount);
        currentDest = destinations[randnum];

    }

    // Update is called once per frame
    void Update()
    {

        direction = player.position - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + raycastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine(StayIdle());
                StopCoroutine(ChaseRoutine());
                StartCoroutine(ChaseRoutine());
                anim.ResetTrigger("walk");
                anim.ResetTrigger("idle");
                anim.SetTrigger("sprint");
                chasing = true;
            }
        }

        if (chasing == true)
        {
            dest = player.position;
            agent.destination = dest;
            agent.speed = chaseSpeed;


            if (agent.remainingDistance <= catchDistance)
            {
                // player.gameObject.SetActive(false);
                anim.ResetTrigger("sprint");
                //anim.SetTrigger("jumpScare");
                //StartCoroutine(DeathRoutine());
                chasing = false;
            }
        }

        if (walking == true)
        {
            dest = currentDest.position;
            agent.destination = dest;
            agent.speed = walkSpeed;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                randnum2 = Random.Range(0, 2);
                if (randnum2 == 0)
                {
                    randnum = Random.Range(0, destinationAmount);
                    currentDest = destinations[randnum];
                }

                if (randnum2 == 1)
                {
                    anim.ResetTrigger("walk");
                    anim.SetTrigger("idle");
                    StopCoroutine(StayIdle());
                    StartCoroutine(StayIdle());
                    walking = false;
                }
            }
        }

    }

    IEnumerator StayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randnum = Random.Range(0, destinationAmount);
        currentDest = destinations[randnum];
        anim.ResetTrigger("idle");
        anim.SetTrigger("walk");
    }

    IEnumerator ChaseRoutine()
    {
        chaseTime = Random.Range(minchaseTime, maxchaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randnum = Random.Range(0, destinationAmount);
        currentDest = destinations[randnum];
        anim.ResetTrigger("sprint");
        anim.SetTrigger("walk");
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(jumpScareTime);
        // SceneManager.LoadScene(deathScene);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + raycastOffset, direction.normalized * sightDistance);
    }
}
