using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Simon : MonoBehaviour
{
    [Header("Game Settings")]
    [Range(0, 4)] public int Lvl = 1;

    public List<int> LvlLength = new List<int> { 2, 3, 4, 5 };
    private List<int> CurrentSequence = new List<int>();

    private int InputNumber = -1;

    [Header("Light Settings")]
    public List<Light> LightsList;   // Assign 4 lights in Inspector
    public float LightIntensity = 50f;

    public float ShowDelay = 0.5f;
    public float BetweenShowDelay = 0.7f;

    private System.Random rnd = new System.Random();

    
    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        Debug.Log("started");
        GenerateSequence();
    }

    private void GenerateSequence()
    {
        if (Lvl <= 0) return;

        int sequenceSize = LvlLength[Lvl - 1];

        while (CurrentSequence.Count < sequenceSize)
        {
            CurrentSequence.Add(rnd.Next(0, 4)); // 0 to 3
        }
        StartCoroutine(ShowSequence());
    }

    public bool CheckInput(int value)
    {
        InputNumber++;
        //Debug.Log(InputNumber);
        //Debug.Log(CurrentSequence);
        StartCoroutine(ShowLight(value));
        
        
        if (CurrentSequence[InputNumber] == value)
        {
            if (InputNumber == CurrentSequence.Count - 1)
            {
                if (Lvl == 4){
                    Victory();
                    return true;
                }
                InputNumber = -1;
                Lvl++;
                GenerateSequence();
            }
            return true;
        }
        else {
            ResetGame();
            return false;
        }
    }

    private void ResetGame()
    {
        Debug.Log("You lose");
        CurrentSequence.Clear();
        InputNumber = -1;
        Lvl = 1;
        GenerateSequence();
    }

    private IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(BetweenShowDelay);

        foreach (int lightId in CurrentSequence)
        {
            ToggleLight(lightId, true);
            yield return new WaitForSeconds(ShowDelay);

            ToggleLight(lightId, false);
            yield return new WaitForSeconds(BetweenShowDelay);
        }
    }

    private IEnumerator ShowLight(int lightId){
        ToggleLight(lightId, true);
        yield return new WaitForSeconds(ShowDelay);
        ToggleLight(lightId, false);

    }

    private void Victory(){
        ToggleLight(0, true);
        ToggleLight(1, true);
        ToggleLight(2, true);
        ToggleLight(3, true);
    }

    private void ToggleLight(int lightId, bool state)
    {
        LightsList[lightId].intensity = state ? LightIntensity : 0f;
        if (state){
            audioData.Play(0);
        }
    }
}
