using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionText : MonoBehaviour
{
    [System.Serializable]
    public class TagTextPair
    {
        public string tag;
        public string displayText;
    }

    [Header("Tag to Text Mapping")]
    public List<TagTextPair> tagTextPairs = new List<TagTextPair>();

    [Header("UI Elements")]
    public TextMeshProUGUI interactionText;

    [Header("Cast Settings")]
    public Camera playerCamera;
    public float interactionRange = 3f;
    public float sphereRadius = 0.5f;
    public Vector3 originOffset = new Vector3(0, -0.25f, 0.5f); // ? small downward + forward offset

    private Dictionary<string, string> tagTextDict;

    void Start()
    {
        tagTextDict = new Dictionary<string, string>();
        foreach (var pair in tagTextPairs)
        {
            if (!tagTextDict.ContainsKey(pair.tag))
                tagTextDict.Add(pair.tag, pair.displayText);
        }
    }

    void Update()
    {
        Vector3 direction = playerCamera.transform.forward;
        Vector3 origin = playerCamera.transform.position + playerCamera.transform.TransformDirection(originOffset);

        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, interactionRange))
        {
            string hitTag = hit.collider.tag;

            if (tagTextDict.ContainsKey(hitTag))
            {
                interactionText.text = tagTextDict[hitTag];
                interactionText.enabled = true;
                return;
            }
        }

        interactionText.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.cyan;
        Vector3 origin = playerCamera.transform.position + playerCamera.transform.TransformDirection(originOffset);
        Vector3 direction = playerCamera.transform.forward * interactionRange;
        Gizmos.DrawRay(origin, direction);
        Gizmos.DrawWireSphere(origin + direction.normalized * interactionRange, sphereRadius);
    }
}
