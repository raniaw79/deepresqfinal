
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
