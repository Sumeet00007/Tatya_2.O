using UnityEngine;

public class FuseRotation : MonoBehaviour
{
    [SerializeField] private string electricPanelTag = "ElectricPanel";
    [SerializeField] private Vector3 rotationOnPanel = new Vector3(90f, 0f, 0f);  // Vertical fit
    [SerializeField] private Vector3 defaultRotation = Vector3.zero;              // Default when held or dropped
    [SerializeField] private float checkRadius = 0.3f;

    private bool isPlacedOnPanel = false;

    private void Update()
    {
        if (IsPlacedOnPanel())
        {
            if (!isPlacedOnPanel)
            {
                transform.localEulerAngles = rotationOnPanel;
                isPlacedOnPanel = true;
            }
        }
        else
        {
            if (isPlacedOnPanel)
            {
                ResetRotation();
                isPlacedOnPanel = false;
            }
        }
    }

    private bool IsPlacedOnPanel()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(electricPanelTag))
            {
                return true;
            }
        }
        return false;
    }

    public void ResetRotation()
    {
        transform.localEulerAngles = defaultRotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
