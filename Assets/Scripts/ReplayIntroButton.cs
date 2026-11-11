
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayIntroButton : MonoBehaviour
{
    public void ReplayIntro()
    {
        PlayerPrefs.SetInt("ReplayIntro", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("IntroScene");
    }
}
