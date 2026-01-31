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

    IEnumerator IncreaseSanite()
    {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            if (AsMask) {
                SaniteLevel++;
            }
            else if (SaniteLevel > 0) {
                SaniteLevel -= 2;
            }
            if (SaniteLevel >= Maxsanite) {
                maxSanite();
            }
            Debug.Log("Sanite Level: " + SaniteLevel);
        }
    }

    void IteractMask()
    {
        AsMask = !AsMask;
        if (AsMask) {
            embiance.SetMaskEmbient();
        }
        else {
            embiance.SetNormalEmbient();
        }
    }
    void maxSanite()
    {
        IteractMask();
    }
}
