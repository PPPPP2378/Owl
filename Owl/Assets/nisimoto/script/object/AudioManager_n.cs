using UnityEngine;

public class AudioManager_n : MonoBehaviour
{
    public static AudioManager_n Instance;

    public bool owlVoice = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            owlVoice = PlayerPrefs.GetInt("OwlVoice", 1) == 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetOwlVoice(bool value)
    {
        owlVoice = value;

        PlayerPrefs.SetInt("OwlVoice", value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
