using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static SettingsManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void ToggleAudio()
    {
        AudioListener.volume = AudioListener.volume == 0f ? 1f : 0f;
        /*if (isMuted) {
            masterBus.setVolume(1);
        } else {
            masterBus.setVolume(0);
        }

        isMuted = !isMuted;*/
    }
}
