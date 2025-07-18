using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_Stats stats;
    private Entity_VFX entityVFX;
    private ElementType currentElement = ElementType.None;
    
    private void Awake()
    {
        stats = GetComponent<Entity_Stats>();
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
    }
    public void ApplyChillEffect(float duration, float slowMultiplier)
    {   float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance); // Apply resistance to the duration

        Debug.Log("Chill effect applied.");
        StartCoroutine(ChillEffectCo(reducedDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentElement = ElementType.Ice;
        entityVFX.PlayOnStatusVFX(duration, ElementType.Ice); // Play chill effect VFX
        yield return new WaitForSeconds(duration); // Duration of the chill effect

        currentElement = ElementType.None; // Reset the element after the effect duration
    }
   public bool CanBeApplied(ElementType element)
    {

        return currentElement == ElementType.None;
    }
}
