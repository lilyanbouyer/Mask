using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class sanite : MonoBehaviour
{

    private int SaniteLevel = 0;
    public int Interval = 1;
    public int Maxsanite = 40;
    public bool AsMask = false;
    public DayNightCycle embiance;

    void Start()
    {
        StartCoroutine(IncreaseSanite());
    }

    private IEnumerator IncreaseSanite()
    {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            if (AsMask) {
                SaniteLevel += 1;
            }
            else if (SaniteLevel > 0) {
                yield return new WaitForSeconds(Interval);
                SaniteLevel -= 1;
            }
            if (SaniteLevel >= Maxsanite) {
                maxSanite();
            }
            Debug.Log("Sanite Level: " + SaniteLevel);
            if (SaniteLevel < 0) {
                SaniteLevel = 0;
            }
        }
    }

    public void IteractMask()
    {
        //shearch for all object with tag "Masked" and toggle their visibility
        GameObject[] maskedObjects = GameObject.FindGameObjectsWithTag("Masked");
        foreach (GameObject obj in maskedObjects) {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null) {
                objRenderer.enabled = !objRenderer.enabled;
            } else {
                Light objLight = obj.GetComponent<Light>();
                if (objLight != null)
                {
                    objLight.enabled = !objLight.enabled;
                }
            }
        }
        AsMask = !AsMask;
        if (AsMask) {
            embiance.SetMaskEmbient();
        }
        else {
            embiance.SetNormalEmbient();
        }
    }
    private void maxSanite()
    {
        IteractMask();
    }
}
