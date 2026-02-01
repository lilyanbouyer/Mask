using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public class Simon : MonoBehaviour
{
    [Header("Game Settings")]
    [Range(0, 4)] public int Lvl = 1;
    public List<int> LvlLength = new List<int> { 2, 3, 4, 5 };
    private List<int> CurrentSequence = new List<int>();
    private int InputNumber = -1;
    
    [Header("Light Settings")]
    public List<Light> LightsList;
    public float LightIntensity = 50f;
    public float ShowDelay = 0.5f;
    public float BetweenShowDelay = 0.7f;
    private System.Random rnd = new System.Random();
    
    [Header("Camera Settings")]
    public Camera playerCamera;
    
    AudioSource audioData;
    private bool sequenceOn;
    private bool isProcessingInput = false; // Empêcher les clics multiples
    
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        Debug.Log("started");
        GenerateSequenceAsync();
    }
    
    private async void GenerateSequenceAsync()
    {
        if (Lvl <= 0) return;
        int sequenceSize = LvlLength[Lvl - 1];
        while (CurrentSequence.Count < sequenceSize)
        {
            CurrentSequence.Add(rnd.Next(0, 4));
        }
        await ShowSequence();
    }
    
    public void CheckRayCast(int buttonId, Vector2 mousePos)
    {
        Ray ray = playerCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Button button = hit.collider.GetComponent<Button>();
            if (button != null && button.buttonId == buttonId)
            {
                CheckInputAsync(buttonId);
            }
        }
    }
    
    private async void CheckInputAsync(int value)
    {
        // Empêcher les clics pendant la séquence ou pendant le traitement d'un input
        if (sequenceOn || isProcessingInput) 
        { 
            Debug.Log("Attendez!");
            return; 
        }
        
        isProcessingInput = true;
        InputNumber++;
        
        // Vérifier que l'index est valide AVANT d'accéder à la liste
        if (InputNumber >= CurrentSequence.Count)
        {
            Debug.LogError($"InputNumber ({InputNumber}) est hors limite! CurrentSequence.Count = {CurrentSequence.Count}");
            isProcessingInput = false;
            return;
        }
        
        // Attendre que ShowLight se termine
        await ShowLight(value);
        
        // Vérification de la réponse
        if (CurrentSequence[InputNumber] == value)
        {
            Debug.Log("Correct!");
            
            // Vérifier si c'est le dernier de la séquence
            if (InputNumber == CurrentSequence.Count - 1)
            {
                if (Lvl == 4)
                {
                    Victory();
                    isProcessingInput = false;
                    return;
                }
                
                // Passer au niveau suivant
                InputNumber = -1;
                Lvl++;
                isProcessingInput = false;
                GenerateSequenceAsync();
                return;
            }
        }
        else
        {
            Debug.Log("Faux!");
            isProcessingInput = false;
            ResetGame();
            return;
        }
        
        isProcessingInput = false;
    }
    
    private void ResetGame()
    {
        Debug.Log("Vous avez perdu");
        CurrentSequence.Clear();
        InputNumber = -1;
        Lvl = 1;
        isProcessingInput = false;
        GenerateSequenceAsync();
    }
    
    private async Task ShowSequence()
    {
        sequenceOn = true;
        InputNumber = -1; // Réinitialiser avant de montrer la séquence
        await Task.Delay((int)(BetweenShowDelay * 1000));
        
        for (int i = 0; i < CurrentSequence.Count; i++)
        {
            int lightId = CurrentSequence[i];
            ToggleLight(lightId, true);
            await Task.Delay((int)(ShowDelay * 1000));
            ToggleLight(lightId, false);
            await Task.Delay((int)(BetweenShowDelay * 1000));
        }
        
        sequenceOn = false;
        Debug.Log($"Séquence terminée. Attendez {CurrentSequence.Count} inputs.");
    }
    
    private async Task ShowLight(int lightId)
    {
        ToggleLight(lightId, true);
        await Task.Delay((int)(ShowDelay * 1000));
        ToggleLight(lightId, false);
    }
    
    private void Victory()
    {
        sequenceOn = true; // Empêcher d'autres inputs
        ToggleLight(0, true);
        ToggleLight(1, true);
        ToggleLight(2, true);
        ToggleLight(3, true);
        Debug.Log("🎉 VICTOIRE! 🎉");
    }
    
    private void ToggleLight(int lightId, bool state)
    {
        if (lightId < 0 || lightId >= LightsList.Count || LightsList[lightId] == null)
        {
            Debug.LogError($"ID de lumière invalide: {lightId}");
            return;
        }
        
        LightsList[lightId].intensity = state ? LightIntensity : 0f;
        if (state)
        {
            audioData.Play(0);
        }
    }
}