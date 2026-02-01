using UnityEngine;
using System.Collections.Generic;
using System.Threading;
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
    
    [Header("Inactivity Settings")]
    public float InactivityTime = 5f; // Time before replaying sequence
    
    public AudioSource audioData;
    public List<AudioClip> audios = new List<AudioClip> {}; // 4 audio
    private bool sequenceOn;
    private bool activated = false;
    private bool isProcessingInput = false;
    private float lastInputTime;
    private CancellationTokenSource inactivityCancellation;
    
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        lastInputTime = Time.time;
    }
    
    void Update()
    {
        // Check for inactivity and replay sequence if needed
        if (activated && !sequenceOn && !isProcessingInput) // && InputNumber >= 0)
        {
            if (Time.time - lastInputTime > InactivityTime)
            {
                ReplaySequence();
            }
        }
    }
    
    // Call this method to activate the Simon game
    public void Activate()
    {
        if (activated)
        {
            return;
        }
        
        activated = true;
        GenerateSequenceAsync();
    }
    
    // Call this to deactivate
    public void Deactivate()
    {
        activated = false;
        sequenceOn = false;
        isProcessingInput = false;
    }

    private async void GenerateSequenceAsync()
    {
        if (!activated || Lvl <= 0) return;
        
        int sequenceSize = LvlLength[Lvl - 1];
        while (CurrentSequence.Count < sequenceSize)
        {
            CurrentSequence.Add(rnd.Next(0, 4));
        }
        await ShowSequence();
    }
    
    private async void ReplaySequence()
    {
        if (!activated || sequenceOn) return;
        
        InputNumber = -1; // Reset input counter
        await ShowSequence();
    }
    
    public void CheckRayCast(int buttonId, Vector2 mousePos)
    {
        if (!activated)
        {
            return;
        }
        
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
        if (!activated)
        {
            return;
        }
        
        // Empêcher les clics pendant la séquence ou pendant le traitement d'un input
        if (sequenceOn || isProcessingInput) 
        { 
            return; 
        }
        
        isProcessingInput = true;
        InputNumber++;
        lastInputTime = Time.time; // Update last input time
        
        // Vérifier que l'index est valide AVANT d'accéder à la liste
        if (InputNumber >= CurrentSequence.Count)
        {
            isProcessingInput = false;
            return;
        }
        
        // Attendre que ShowLight se termine
        await ShowLight(value);
        
        // Vérification de la réponse
        if (CurrentSequence[InputNumber] == value)
        {
            
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
            isProcessingInput = false;
            ResetGame();
            return;
        }
        
        isProcessingInput = false;
    }
    
    private void ResetGame()
    {
        CurrentSequence.Clear();
        InputNumber = -1;
        Lvl = 1;
        isProcessingInput = false;
        lastInputTime = Time.time;
        GenerateSequenceAsync();
    }
    
    private async Task ShowSequence()
    {
        if (!activated) return;
        
        sequenceOn = true;
        InputNumber = -1; // Réinitialiser avant de montrer la séquence
        lastInputTime = Time.time; // Reset inactivity timer
        
        await Task.Delay((int)(BetweenShowDelay * 1000));
        
        for (int i = 0; i < CurrentSequence.Count; i++)
        {
            if (!activated) break; // Stop if deactivated
            
            int lightId = CurrentSequence[i];
            ToggleLight(lightId, true);
            await Task.Delay((int)(ShowDelay * 1000));
            ToggleLight(lightId, false);
            await Task.Delay((int)(BetweenShowDelay * 1000));
        }
        
        sequenceOn = false;
        lastInputTime = Time.time; // Start inactivity timer
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
        activated = false; // Désactiver le jeu
        ToggleLight(0, true);
        ToggleLight(1, true);
        ToggleLight(2, true);
        ToggleLight(3, true);
    }
    
    private void ToggleLight(int lightId, bool state)
    {
        if (lightId < 0 || lightId >= LightsList.Count || LightsList[lightId] == null)
        {
            Debug.LogError($"ID de lumière invalide: {lightId}");
            return;
        }
        
        LightsList[lightId].intensity = state ? LightIntensity : 0f;
        if (state && activated)
        {
            audioData.PlayOneShot(audios[lightId]);
        }
    }
}