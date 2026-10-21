
using UnityEngine;

public class PersistentOceanAudio : MonoBehaviour
{
    private static PersistentOceanAudio instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
