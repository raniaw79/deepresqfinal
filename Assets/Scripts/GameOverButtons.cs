
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverButtons : MonoBehaviour
{
    [Header("Button Sounds")]
    public AudioSource restartAudio;
    public AudioSource homeAudio;

    private bool isLoading = false;

    public void RestartGame()
    {
        StartCoroutine(LoadSceneWithSound("Level1", restartAudio));
    }

    public void GoToHome()
    {
        StartCoroutine(LoadSceneWithSound("Home", homeAudio));
    }

    public void PlayHomeButtonSound()
    {
        StartCoroutine(LoadSceneWithSound("Home", homeAudio));
    }

    private IEnumerator LoadSceneWithSound(string sceneName, AudioSource audioSource)
    {
        if (isLoading)
            yield break;

        isLoading = true;

        Debug.Log("Button pressed. Target scene: " + sceneName);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing for scene: " + sceneName);
        }
        else if (audioSource.clip == null)
        {
            Debug.LogError("AudioClip is missing for scene: " + sceneName);
        }
        else
        {
            audioSource.Play();
            Debug.Log("Playing sound for: " + sceneName);

            yield return new WaitForSecondsRealtime(audioSource.clip.length);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}