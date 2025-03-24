using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderJumpScare : MonoBehaviour
{
    public GameObject spiderPrefab;
    public Transform spawnPoint;
    private bool hasSpawned = false;

    public Transform targetPosition;
    public float speed = 7f;

    private Vector3 initialPosition;

    public AudioSource spiderScare;

    void Start()
    {
        initialPosition = transform.position;
    }



    public void EnableSpiderJumpScare()
    {
        if (!hasSpawned && spiderPrefab != null)
        {
            spiderScare.Play();

            GameObject spider = Instantiate(spiderPrefab, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(MoveSpider(spider));
            hasSpawned = true;
        }
    }


    IEnumerator MoveSpider(GameObject spider)
    {
        while (spider != null && Vector3.Distance(spider.transform.position, targetPosition.position) > 0.1f)
        {
            spider.transform.position = Vector3.MoveTowards(spider.transform.position, targetPosition.position, speed * Time.deltaTime);
            yield return null; 
        }

        if (spider != null)
        {
            Destroy(spider);
        }
    }
  
}
