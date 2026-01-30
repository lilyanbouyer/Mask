using UnityEngine;
using System.Linq;

public class LightController : MonoBehaviour
{
    public Light[] controlledLights;

    void Start()
    {
        // get the lights in the scene it should have the tag "switchable"
        controlledLights = GameObject.FindGameObjectsWithTag("switchable").Select(go => go.GetComponent<Light>()).ToArray();
    }

    private string extractItemSuffix(string item_name)
    {
        return (item_name[item_name.Length - 2].ToString() + item_name[item_name.Length - 1].ToString());
    }

    public bool IsGoodItem(string PlacementName, string ItemName)
    {
        return extractItemSuffix(PlacementName) == (extractItemSuffix(ItemName));
    }

    public void ToggleLightWithend(string SwitchName, GameObject SwitchObject)
    {
        for (int i = 0; i < controlledLights.Length; i++)
        {
            if (extractItemSuffix(controlledLights[i].name) == extractItemSuffix(SwitchName))
            {
                controlledLights[i].enabled = !controlledLights[i].enabled;
            }
        }
        SwitchObject.transform.Rotate(0f, 0f, 180f);
    }

    // Toggle all lights when index is -1, otherwise toggle the specific light by index.
    public void ToggleLight(int index = -1)
    {
        if(index == -1)
        {
            foreach(var light in controlledLights)
            {
                light.enabled = !light.enabled;
            }
        }
        else if(index >= 0 && index < controlledLights.Length)
        {
            controlledLights[index].enabled = !controlledLights[index].enabled;
        }
    }
}
