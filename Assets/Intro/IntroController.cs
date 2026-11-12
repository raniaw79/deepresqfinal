
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string homeSceneName = "Home";

    private const string IntroSeenKey = "IntroAlreadySeen";
    private const string ReplayKey = "ReplayIntro";
    private static string pendingGameScene;
    private string nextScene;

    public static void StartGame()
    {
        if (PlayerPrefs.GetInt(IntroSeenKey, 0) == 1)
        {
            SceneManager.LoadScene("Story");
            return;
        }

        pendingGameScene = "Story";
        SceneManager.LoadScene("IntroScene");
    }

    private void Start()
    {
        nextScene = pendingGameScene ?? homeSceneName;
        pendingGameScene = null;
        bool hasSeenIntro = PlayerPrefs.GetInt(IntroSeenKey, 0) == 1;
        bool replayRequested = PlayerPrefs.GetInt(ReplayKey, 0) == 1;

        if (hasSeenIntro && !replayRequested)
        {
            SceneManager.LoadScene(nextScene);
            return;
        }

        PlayerPrefs.SetInt(ReplayKey, 0);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.aspectRatio = VideoAspectRatio.Stretch;

        // Hide the previous frame until the decoder has prepared this playback.
        var image = videoPlayer.GetComponent<RawImage>();
        if (image != null)
            image.enabled = false;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer player)
    {
        var image = player.GetComponent<RawImage>();
        if (image != null)
        {
            var fitter = image.GetComponent<AspectRatioFitter>();
            if (fitter != null)
                fitter.enabled = false;

            // Stretch the complete frame to the canvas on phones and tablets.
            var rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one;
            image.uvRect = new Rect(0f, 0f, 1f, 1f);
            image.enabled = true;
        }

        player.Play();
    }

    private void OnVideoFinished(VideoPlayer player)
    {
        PlayerPrefs.SetInt(IntroSeenKey, 1);
        PlayerPrefs.SetInt(ReplayKey, 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nextScene);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.prepareCompleted -= OnVideoPrepared;
        }
    }
}
