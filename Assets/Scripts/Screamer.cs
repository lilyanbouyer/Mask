using UnityEngine;

public class Screamer : MonoBehaviour
{
    public AudioSource screamerAudio;
    public AudioClip screamerClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (screamerAudio != null && screamerClip != null)
            {
                screamerAudio.PlayOneShot(screamerClip);
            }
        }
    }
}
