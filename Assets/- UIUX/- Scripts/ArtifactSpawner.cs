using System.Collections;
using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    public GameObject[] artifacts;
    [SerializeField]
    private float spwanTimer = 3.0f;


    // Update is called once per frame
    void Start()
    {
        StartCoroutine(SpawnArtifactsRoutine());
    }

    IEnumerator SpawnArtifactsRoutine()
    {
        while (true)
        {
            SpawnArtifact();
            yield return new WaitForSeconds(spwanTimer);
        }
      
    }

    private void SpawnArtifact()
    {
        if (artifacts.Length == 0) return; 

        int randomIndex = Random.Range(0, artifacts.Length); 
        GameObject spawningArtifact = artifacts[randomIndex];

        Instantiate(spawningArtifact, transform.position, Quaternion.identity);
    }
}
