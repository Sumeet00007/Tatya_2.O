using System.Collections;
using UnityEngine;

public class RopeRemover : MonoBehaviour
{
    public GameObject[] ropes;
    public float fadeDuration = 1.5f;
    public Color targetEmissionColor = Color.red; // horror red glow
    public AudioSource playerSource;
    public AudioClip ropeCutSound;

    void Start()
    {
        foreach (GameObject rope in ropes)
        {
            rope.SetActive(true);
        }
    }

    public void RemoveRopes(int ropeIndex)
    {
        if (ropeIndex < 0 || ropeIndex >= ropes.Length) return;
        StartCoroutine(FadeOutRope(ropes[ropeIndex]));

        if (playerSource != null && ropeCutSound != null)
        {
            playerSource.PlayOneShot(ropeCutSound);
        }
    }

    private IEnumerator FadeOutRope(GameObject rope)
    {
        Renderer rend = rope.GetComponent<Renderer>();
        if (rend == null) yield break;

        // Create a property block to override emission color safely
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block);

        Color startEmission = rend.sharedMaterial.GetColor("_EmissionColor");
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Lerp emission color toward red
            Color newEmission = Color.Lerp(startEmission, targetEmissionColor, t);
            block.SetColor("_EmissionColor", newEmission * Mathf.LinearToGammaSpace(1.5f));

            rend.SetPropertyBlock(block);
            yield return null;
        }

        // Small delay for effect, then hide the rope
        yield return new WaitForSeconds(0.2f);
        rope.SetActive(false);
    }
}
