using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPlacement : MonoBehaviour
{
    public string requiredItemTag = "Pickup"; // Tag of the item that can be placed
    public GameObject playerHead; // Assign this in the Inspector for raycasting
    public EnigmeChecker enigmeChecker; // Add this field

    private GameObject currentItem;
    public float placementRayDistance = 4f; // Maximum distance for detecting a placement


    private string extractItemSuffix(string item_name)
    {
        return (item_name[item_name.Length - 2].ToString() + item_name[item_name.Length - 1].ToString());
    }

    public bool IsGoodItem(string PlacementName, string ItemName)
    {
        return extractItemSuffix(PlacementName) == (extractItemSuffix(ItemName));
    }

    public void PlaceItem(RaycastHit hit)
    {
        if(hit.transform.childCount > 0)
        {
            return;
        }
        currentItem.transform.SetParent(hit.transform);
        // Place the item at the center of the placement but 3 units down
        currentItem.transform.localPosition = new Vector3(0, -1, 0);
        currentItem.transform.rotation = hit.transform.rotation * Quaternion.Euler(0, 180, 0);
        // currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Rigidbody>().useGravity = false;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = true;
        if(enigmeChecker != null && IsGoodItem(hit.transform.name, currentItem.name))
        {
            enigmeChecker.IncrementCurrent();
        }
        currentItem = null;
    }

    public void SetCurrentItem(GameObject item)
    {
        currentItem = item;
    }
}

