using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SetStaticAndLightProbes : MonoBehaviour
{
    [MenuItem("Tools/Set All Objects to Static and Light Probes")]
    private static void SetObjectsToStaticAndLightProbes()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            // Set object to Static
            obj.isStatic = true;

            // Get MeshRenderer and update light probe usage
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
                meshRenderer.receiveGI = ReceiveGI.LightProbes;
            }
        }

        Debug.Log("All GameObjects set to Static and MeshRenderers updated to use Light Probes instead of Lightmaps.");
    }
}
