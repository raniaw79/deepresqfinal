
using UnityEngine;

public class VictoryPanelSound : MonoBehaviour
{
    private AudioSource victoryAudio;
    private bool hasPlayed = false;

    private void Awake()
    {
        victoryAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (hasPlayed)
            return;

        if (victoryAudio != null && victoryAudio.clip != null)
        {
            victoryAudio.Play();
            hasPlayed = true;
        }
    }
}
