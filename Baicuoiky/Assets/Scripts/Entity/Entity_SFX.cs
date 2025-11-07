using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;


    [Header("SFX Name")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;
    [SerializeField] private string walkStone;
    [SerializeField] private string dash;
    [SerializeField] private string domainExpand;

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
    public void PlayWalkStone()
    {
        Audio_Manager.instance.PlaySFX(walkStone, audioSource, soundDistance);
    }
    public void PlayDash()
    {
        Audio_Manager.instance.PlaySFX(dash, audioSource, soundDistance);
    }
    public void PlayDomainExpand()
    {
        Audio_Manager.instance.PlaySFX(domainExpand, audioSource, soundDistance);
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
