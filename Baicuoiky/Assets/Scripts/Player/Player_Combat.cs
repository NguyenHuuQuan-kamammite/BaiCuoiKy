using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = .1f;
     [SerializeField] private LayerMask whatIsCounterable;
    public bool CounterAttackPerform()
    {
        bool hasCounteredSomebody = false;
        
        foreach (var target in GetDetectedColliders(whatIsCounterable))
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue; //skip this target, goes to next target

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasCounteredSomebody = true;
            }

        }
        return hasCounteredSomebody;
    }
    public float GetCounterRecoveryDuration() => counterRecovery;
}

