using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;
    [Header("On Doing Damage VFX")]
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject critHitVfx;
    [Header("Elemental Color")]
    [SerializeField] private Color chillVfx = Color.cyan;
    private Color originaltHitVfxColor;


    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
        originaltHitVfxColor = hitVfxColor;
    }
    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
        {
            StartCoroutine(PlayStatusVfxCO(chillVfx, duration));
        }
        else
        {
            Debug.LogWarning("No status effect VFX defined for this element type.");
        }
    }
   
    private IEnumerator PlayStatusVfxCO(Color effectColor, float duration)
    {
        float tickInterval = 0.25f;
        float timeHasPassed = 0f;
        Color lightColor = effectColor * 1.5f;
        Color darkColor = effectColor * 0.8f;
        bool toggle = false;
        while (timeHasPassed < duration)
        {

            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;
            yield return new WaitForSeconds(tickInterval);
            timeHasPassed = timeHasPassed + tickInterval;
        }
        sr.color = Color.white; // Reset to original color
    }


    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        if (hitVfx == null) return;
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0f, 180f, 0f);
    }
    public void UpdateOnHitColor(ElementType element)
    {
      if (element == ElementType.Ice)
        {
            hitVfxColor = chillVfx;
        }
     if (element == ElementType.None )
        {
            hitVfxColor = originaltHitVfxColor;
        }
      
    }
   
    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
        {
            StopCoroutine(onDamageVFXCoroutine);
        }

        StartCoroutine(OnDamageVFXCoroutine());
    }
    private IEnumerator OnDamageVFXCoroutine()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
    



}
