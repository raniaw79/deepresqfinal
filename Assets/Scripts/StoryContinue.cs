
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryContinue : MonoBehaviour
{
    public void GoToLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}
