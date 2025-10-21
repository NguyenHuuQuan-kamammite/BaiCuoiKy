using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;


    [Header("SFX Name")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;
    [Space]
    [SerializeField] private float soundDistance =15;
    [SerializeField] private bool showGizmo;
    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();

    }

    public void PlayAttackHit()
    {
        Audio_Manager.instance.PlaySFX(attackHit,audioSource, soundDistance);

    }
    public void PlayAttackMiss()
    {
        Audio_Manager.instance.PlaySFX(attackMiss, audioSource, soundDistance);
    }
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, soundDistance);
        }

    }
}
