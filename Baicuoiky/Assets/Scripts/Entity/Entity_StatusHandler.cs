using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_Stats stats;
    private Entity_Health entityHealth;
    private Entity_VFX entityVFX;
    private ElementType currentElement = ElementType.None;
    [Header("Shock Effect Details")]
    [SerializeField] private GameObject electricVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 1f;
    private Coroutine shockCo;
    private void Awake()
    {
        stats = GetComponent<Entity_Stats>();
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        entityHealth = GetComponent<Entity_Health>();
    }
    public void ApplyChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1 - iceResistance); // Apply resistance to the duration

        Debug.Log("Chill effect applied.");
        StartCoroutine(ChillEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentElement = ElementType.Ice;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Ice); // Play chill effect VFX
        yield return new WaitForSeconds(duration); // Duration of the chill effect

        currentElement = ElementType.None; // Reset the element after the effect duration
    }
    public void ApplyBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireResistance); // Apply resistance to the duration

        Debug.Log("Burn effect applied.");
        StartCoroutine(BurnEffectCo(duration, finalDamage));
    }
    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentElement = ElementType.Fire;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Fire); // Play burn effect VFX

        int tickersPerSecond = 2; // Number of damage ticks per second
        int tickCount = Mathf.RoundToInt(duration * tickersPerSecond);
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / tickersPerSecond;


        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHp(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentElement = ElementType.None; // Reset the element after the effect duration
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
            ApplyChillEffect(effectData.chillDuration, effectData.chillSlowMultiplier); // Apply chill effect

        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
            ApplyBurnEffect(effectData.burnDuration, effectData.totalBurnDamage); // Apply burn effect
        
        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
            ApplyShockEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge); // Apply electric effect

    }

    public void ApplyShockEffect(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance); // Apply resistance to the charge

        currentCharge = currentCharge + finalCharge;
        if (currentCharge >= maxCharge)
        {
            DoLightningStrike(damage);
            StopShockEffect();
            return;
        }


        if (shockCo != null)
        {
            StopCoroutine(shockCo);
        }
        shockCo = StartCoroutine(ShockEffectCo(duration));

    }
    private void StopShockEffect()
    {
        currentElement = ElementType.None;
        currentCharge = 0f;
        entityVFX.StopAllVfx(); // Stop any ongoing VFX
    }
    private void DoLightningStrike(float damage)
    {
        Instantiate(electricVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHp(damage);
    }
    
     private IEnumerator ShockEffectCo(float duration)
    {
        currentElement = ElementType.Lightning;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Lightning); // Play electric effect VFX
        yield return new WaitForSeconds(duration); // Duration of the electric effect
        

        StopShockEffect();
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentElement == ElementType.Lightning)
        {
            return true; // Lightning effect is already applied
        }
        return currentElement == ElementType.None;
    }
}
