using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;


    [Header("SFX Name")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();

    }

    public void PlayAttackHit()
    {
        Audio_Manager.instance.PlaySFX(attackHit,audioSource);

    }
    public void PlayAttackMiss()
    {
        Audio_Manager.instance.PlaySFX(attackMiss, audioSource);
    }
}
