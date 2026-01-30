using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public Transform playerHand; // Assign this in the Inspector
    public GameObject playerHead; // Assign this in the Inspector
    private GameObject currentItem;
    void Start()
    {
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }

    public void SetCurrentItem(GameObject item)
    {
        currentItem = item;
    }

    public void PickupItem(RaycastHit hit)
    {
        GameObject targetItem = hit.transform.gameObject;
        if(targetItem.transform.parent != null && targetItem.transform.parent.CompareTag("Placement"))
        {
            ItemPlacement placement = targetItem.transform.parent.GetComponent<ItemPlacement>();
            if(placement.enigmeChecker != null && placement.IsGoodItem(targetItem.transform.parent.name, targetItem.name))
            {
                placement.enigmeChecker.DecrementCurrent();
            }
            targetItem.transform.SetParent(null);
        }
        currentItem = targetItem;
        currentItem.transform.SetParent(playerHand);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.Euler(0, 180, 0);
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        currentItem.GetComponent<Rigidbody>().useGravity = true;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = false;
        if (hit.transform.parent.GetComponent<ItemPlacement>())
            hit.transform.parent.GetComponent<ItemPlacement>().SetCurrentItem(currentItem);
}

    public void DropItem()
    {
        if (false) {
        currentItem.transform.SetParent(null);
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Rigidbody>().useGravity = true;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = true;
        currentItem = null;
        }
    }
}
