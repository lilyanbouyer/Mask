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
    public AudioSource soundManager;
    public AudioClip soundMaskOn;
    public AudioClip soundScream;

    void Start()
    {
        StartCoroutine(IncreaseSanite());
    }

    private IEnumerator IncreaseSanite()
    {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            if (SaniteLevel >= Maxsanite -28 && AsMask && soundManager.isPlaying == false)
            {
                soundManager.PlayOneShot(soundMaskOn);
            }
            if (AsMask) {
                SaniteLevel += 1;
            } else if (SaniteLevel > 0) {
                yield return new WaitForSeconds(Interval);
                SaniteLevel -= 1;
            }
            if (SaniteLevel == Maxsanite && AsMask) {
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
        if (AsMask || SaniteLevel == 0) {
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
                soundManager.Stop();
            }
        }
    }

    private void maxSanite()
    {
        soundManager.Stop();
        if (soundScream != null && soundManager.isPlaying == false)
        {
            soundManager.PlayOneShot(soundScream);
        }
        //IteractMask();
    }
}
