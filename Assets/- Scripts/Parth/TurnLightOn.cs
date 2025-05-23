using System.Linq;
using UnityEngine;

public class TurnLightOn : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Light pointLight;
    public AudioSource switchONSound;
    [SerializeField] ElectricPanelChecker itemChecker;

    bool lightIsOn = false;

    void Start()
    {
        pointLight.enabled = false;
    }

    public void OnCompletion()
    {
        // This will still play when the correct combination is first completed
       
        Debug.Log("Correct Items");
        pointLight.enabled = true;
        lightIsOn = true;
    }

    void Update()
    {
        // Continuously check if the correct combination is still present
        if (IsCombinationCorrect())
        {
            if (!lightIsOn)
            {
                pointLight.enabled = true;
                lightIsOn = true;
                switchONSound.Play();
            }
        }
        //else
        //{
        //    if (lightIsOn)
        //    {
        //        pointLight.enabled = false;
        //        lightIsOn = false;
        //    }
        //}
    }

    // Reuse the same check logic similar to RequiredItemsChecker
    bool IsCombinationCorrect()
    {
        var itemsPositions = itemChecker.GetItemsPosition();
        var checkRadius = itemChecker.GetCheckSphereRadius();
        var itemLayer = itemChecker.GetItemLayerMask();

        var itemsNeeded = new System.Collections.Generic.List<string>();
        foreach (var obj in itemChecker.itemsNeededGameObjects)
        {
            itemsNeeded.Add(obj.name);
        }
        itemsNeeded.Sort();

        var itemsDeposited = new System.Collections.Generic.List<string>();

        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            Collider itemInSphere = Physics.OverlapSphere(itemsPositions[i].position, checkRadius, itemLayer).FirstOrDefault();
            if (itemInSphere != null)
            {
                itemsDeposited.Add(itemInSphere.gameObject.name.Replace("(Clone)", ""));
            }
            else
            {
                return false; // Missing item
            }
        }

        itemsDeposited.Sort();

        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            if (itemsDeposited[i] != itemsNeeded[i])
            {
                return false; // Incorrect item
            }
        }

        return true; // All correct
    }

    public void LightOff()
    {
        pointLight.enabled = false;
        lightIsOn = false;
        switchONSound.Play();
    }

    public void LightOn()
    {
        pointLight.enabled = true;
        lightIsOn = true;
    }
}
