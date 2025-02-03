using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DepositeSite : MonoBehaviour
{
    [SerializeField] Transform[] itemsPosition;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] float checkSphereRadius = 0.2f;
    [SerializeField] List<string> itemsNeeded;

    bool[] isSlotFilled;
    List<string> itemsDeposited;

    void Start()
    {
        isSlotFilled = new bool[itemsPosition.Length];
        itemsDeposited = new List<string>();
    }

    void Update()
    {
        CheckIfCanCombineItems();
    }

    void CheckIfCanCombineItems()
    {
        itemsDeposited.Clear();

        for (int i = 0; i < itemsPosition.Length; i++)
        {
            isSlotFilled[i] = Physics.CheckSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask);
            if (isSlotFilled[i])
            {
                Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
                if (itemInSphere != null)
                {
                    itemsDeposited.Add(itemInSphere.gameObject.name);
                }
            }
        }
    }

    public Vector3 GetUnoccupiedPlace()
    {
        for (int i = 0; i < itemsPosition.Length; i++)
        {
            isSlotFilled[i] = Physics.CheckSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask);
            if (!isSlotFilled[i])
            {
                return itemsPosition[i].position;
            }
        }

        return Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (itemsPosition != null)
        {
            foreach (var pos in itemsPosition)
            {
                Gizmos.DrawWireSphere(pos.position, checkSphereRadius);
            }
        }
    }
}
