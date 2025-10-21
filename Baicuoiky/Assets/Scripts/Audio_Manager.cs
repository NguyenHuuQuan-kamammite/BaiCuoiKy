using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager instance;

    [SerializeField] private AudioDataBaseSO audioDB;
    [SerializeField] private AudioSource bmgSource;
    [SerializeField] private AudioSource sfxSource;
    private Transform player;

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

    public void PlaySFX(string soundName, AudioSource sfxSource, float minDistanceToHearSound = 5)
    {

        if(player == null) 
            player = Player.instance.transform;
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
        float maxVolume = data.maxVolume;
        float distance = Vector2.Distance(sfxSource.transform.position,player.position);
        float t = Mathf.Clamp01(1- (distance/minDistanceToHearSound));  

        
        sfxSource.pitch = Random.Range(0.95f, 1.1f);
        sfxSource.volume = Mathf.Lerp(0, maxVolume, t*t);
        sfxSource.PlayOneShot(clip);
    }
    public void PlayGlobalSFX(string soundName)
    {
        var data = audioDB.Get(soundName);
        if (data == null) return;

        var clip = data.GetRandomeClip();
        if (clip == null) return;

        Debug.Log("Played audio " + soundName);

        sfxSource.pitch = Random.Range(.95f, 1.1f);
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
