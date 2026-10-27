
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    public AudioSource buttonSound;

    public void ContinueToLevel2()
    {
        StartCoroutine(PlaySoundAndLoadLevel2());
    }

    private IEnumerator PlaySoundAndLoadLevel2()
    {
        if (buttonSound != null && buttonSound.clip != null)
        {
            buttonSound.Play();
            yield return new WaitForSecondsRealtime(buttonSound.clip.length);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
}
