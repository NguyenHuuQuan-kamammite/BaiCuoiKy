using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    [SerializeField] private AudioDataBaseSO audioDB;
    [SerializeField] private AudioSource bmgSource;
    [SerializeField] private AudioSource sfxSource;

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

    public void PlaySFX(string soundName, AudioSource sfxSource)
    {
        var data = audioDB.Get(soundName);
        if (data == null)
        {
            Debug.Log("Attepm to play sound " + soundName);
            return;
        }

        var clip = data.GetRandomeClip();
        if (clip == null)
        {
            return;
        }

        sfxSource.clip = clip;
        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.volume = data.volume;
        sfxSource.PlayOneShot(clip);
    }
}
