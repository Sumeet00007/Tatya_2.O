using UnityEngine;

public class RopeRemover : MonoBehaviour
{
    public GameObject[] ropes;
    void Start()
    {
        for(int i = 0; i < ropes.Length; i++)
        {
            ropes[i].gameObject.SetActive(true);
        }
    }


    public void RemoveRopes(int ropeIndex)
    {
        ropes[ropeIndex].SetActive(false);
    }
  
}
