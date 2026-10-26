
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeButton : MonoBehaviour
{
    public AudioSource buttonSound;

    public void GoToHome()
    {
        StartCoroutine(PlaySoundAndLoadHome());
    }

    private IEnumerator PlaySoundAndLoadHome()
    {
        if (buttonSound != null && buttonSound.clip != null)
        {
            buttonSound.Play();
            yield return new WaitForSecondsRealtime(buttonSound.clip.length);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }

    public void PlayGame()
    {
        StartCoroutine(PlaySoundAndLoadStory());
    }

    private IEnumerator PlaySoundAndLoadStory()
    {
        if (buttonSound != null && buttonSound.clip != null)
        {
            buttonSound.Play();
            yield return new WaitForSecondsRealtime(buttonSound.clip.length);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Story");
    }
}
