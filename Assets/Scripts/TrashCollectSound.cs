
using UnityEngine;

public class TrashCollectSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayCollectSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning("Trash collect AudioSource is not assigned!");
        }
    }
}
