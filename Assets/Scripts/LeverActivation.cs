using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public class LeverActivation : MonoBehaviour
{
    [Header("Lever Settings")]
    public Vector3 ActivatedRotation = new Vector3(90.0f, 0f, 0f);
    public Vector3 BaseRotation = Vector3.zero;
    public float smoothSpeed = 2f;
    public GameObject QTELinked;
    public Simon simon;
    
    [Header("Invert Axes")]
    [Tooltip("Invert the opening direction on X axis relative to CloseRotation")]
    public bool invertX = false;
    [Tooltip("Invert the opening direction on Y axis relative to CloseRotation")]
    public bool invertY = false;
    [Tooltip("Invert the opening direction on Z axis relative to CloseRotation")]
    public bool invertZ = false;
    
    private bool isOpening = false;
    private Quaternion targetRotation;
    private QTEActivated qteScript;
    private bool InGame = false;
    public AudioSource audioData;
    public List<AudioClip> audios = new List<AudioClip> {};

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        if (QTELinked != null)
        {
            qteScript = QTELinked.transform.GetComponent<QTEActivated>();
        }
    }
    
    public bool TryOpen()
    {
        if (isOpening)
        {
            return false;
        }
        
        if (QTELinked != null && qteScript != null && !InGame)
        {
            if (!qteScript.IsActive()) {
                InGame = true;
                qteScript.StartQTE();
                return true;
            }
            else {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
    public void Update()
    {
        // Check if QTE is completed
        if (qteScript != null && qteScript.done && !isOpening)
        {
            ActivateLever();
            simon.Activate();
        }
        
        Vector3 targetEuler = isOpening ? GetEffectiveOpenRotation() : BaseRotation;
        targetRotation = Quaternion.Euler(targetEuler);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }

    public async void ActivateLever()
    {
        InGame = false;
        isOpening = true;
    
        audioData.PlayOneShot(audios[0]);
        await Task.Delay(1000);
        audioData.PlayOneShot(audios[1]);
    
        if (audios[1] != null)
        {
            await Task.Delay((int)(audios[1].length * 1000));
        }
    
        if (audios[2] != null)
        {
            audioData.clip = audios[2];
            audioData.loop = true;
            audioData.Play();
        }
    }
    
    public bool IsOpen()
    {
        return isOpening;
    }
    
    private Vector3 GetEffectiveOpenRotation()
    {
        Vector3 effective = ActivatedRotation;
        if (invertX)
            effective.x = BaseRotation.x - (ActivatedRotation.x - BaseRotation.x);
        if (invertY)
            effective.y = BaseRotation.y - (ActivatedRotation.y - BaseRotation.y);
        if (invertZ)
            effective.z = BaseRotation.z - (ActivatedRotation.z - BaseRotation.z);
        return effective;
    }
}