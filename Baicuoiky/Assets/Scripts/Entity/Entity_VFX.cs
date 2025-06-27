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
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        entity = GetComponent<Entity>();
        originalMaterial = sr.material;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit)
    {
        if (hitVfx == null) return;
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        if(isCrit == false)
            vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0f, 180f, 0f);
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
