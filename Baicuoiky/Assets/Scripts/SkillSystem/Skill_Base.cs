using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General details")]
    [SerializeField] private float coolDown;
    [SerializeField] protected Skill_Type skillType;
    [SerializeField] protected SkillUnlock_Type unlockType;
    private float lastTimeUsed;
    protected virtual void Awake()
    {
        lastTimeUsed = -coolDown; // So that the skill can be used immediately at the start
    }
    public void SetSkillUnlock(SkillUnlock_Type unlock)
    {
       unlockType = unlock;
    }
    public bool CanUseSkill()
    {
        if (OnCoolDown())
        {
            Debug.Log("Skill on cooldown");
            return false;
        }
        return true;
    }

    private bool OnCoolDown() => Time.time < lastTimeUsed + coolDown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float coolDownReduction) => lastTimeUsed = lastTimeUsed + coolDownReduction;

    public void ResetCoolDown() => lastTimeUsed = Time.time;
}
