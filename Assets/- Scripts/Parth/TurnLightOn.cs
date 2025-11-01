using System.Linq;
using UnityEngine;

public class TurnLightOn : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Light pointLight;
    public AudioSource fuseSource;
    public AudioClip electricONSound;
    [SerializeField] ElectricPanelChecker itemChecker;
    [SerializeField] MeshRenderer bulbRenderer; // ✅ added

    bool lightIsOn = false;

    void Start()
    {
        pointLight.enabled = false;

        if (bulbRenderer != null)
            bulbRenderer.enabled = false; // ✅ ensure bulb starts off
    }

    public void OnCompletion()
    {
        Debug.Log("Correct Items");
        pointLight.enabled = true;
        if (bulbRenderer != null)
            bulbRenderer.enabled = true; // ✅ turn bulb on
        lightIsOn = true;
    }

    void Update()
    {
        if (IsCombinationCorrect())
        {
            if (!lightIsOn)
            {
                pointLight.enabled = true;
                if (bulbRenderer != null)
                    bulbRenderer.enabled = true; // ✅ turn bulb on
                lightIsOn = true;
                fuseSource.PlayOneShot(electricONSound);
            }
        }
        //else block intentionally commented out
    }

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

        return true;
    }

    public void LightOff()
    {
        pointLight.enabled = false;
        if (bulbRenderer != null)
            bulbRenderer.enabled = false; // ✅ turn bulb off
        lightIsOn = false;
        fuseSource.PlayOneShot(electricONSound);
    }

    public void LightOn()
    {
        pointLight.enabled = true;
        if (bulbRenderer != null)
            bulbRenderer.enabled = true; // ✅ turn bulb on
        lightIsOn = true;
    }
}
