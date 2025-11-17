using UnityEngine;

public class ExplosionVFX_SFX : MonoBehaviour
{
    [SerializeField] private string explosionSFX = "explosion";
    [SerializeField] private float soundDistance = 15f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        Audio_Manager.instance.PlaySFX(explosionSFX, audioSource, soundDistance);
    }
}

