
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string homeSceneName = "Home";

    private const string IntroSeenKey = "IntroAlreadySeen";
    private const string ReplayKey = "ReplayIntro";

    private void Start()
    {
        bool hasSeenIntro = PlayerPrefs.GetInt(IntroSeenKey, 0) == 1;
        bool replayRequested = PlayerPrefs.GetInt(ReplayKey, 0) == 1;

        if (hasSeenIntro && !replayRequested)
        {
            SceneManager.LoadScene(homeSceneName);
            return;
        }

        PlayerPrefs.SetInt(ReplayKey, 0);
        PlayerPrefs.Save();

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer player)
    {
        PlayerPrefs.SetInt(IntroSeenKey, 1);
        PlayerPrefs.SetInt(ReplayKey, 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(homeSceneName);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
