
using UnityEngine;

public class BackstoryButtonSound : MonoBehaviour
{
    public AudioSource buttonSound;

    public void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }
}
