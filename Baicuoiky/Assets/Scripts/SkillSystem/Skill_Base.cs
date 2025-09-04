using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public DamageScaleData damageScaleData { get; private set; }
    public Player player { get; private set; }
    [Header("General details")]
    [SerializeField] protected float coolDown;
    [SerializeField] protected Skill_Type skillType;
    [SerializeField] protected SkillUnlock_Type unlockType;
    private float lastTimeUsed;
    protected virtual void Awake()
    {

        player = GetComponentInParent<Player>();
        lastTimeUsed = -coolDown; // So that the skill can be used immediately at the start
    }


    public virtual void TryToUseSkill()
    {
        
    }
    public void SetSkillUnlock(UpgradeData unlock)
    {
        unlockType = unlock.upgradeType;
        coolDown = unlock.cooldown;
        damageScaleData = unlock.damageScale;
    }
    public bool CanUseSkill()
    {
        if (unlockType == SkillUnlock_Type.None)
        {
            return false;
        }
        if (OnCoolDown())
        {
            Debug.Log("Skill on cooldown");
            return false;
        }
        return true;
    }

protected bool Unlocked(SkillUnlock_Type upgradeToCheck) => unlockType == upgradeToCheck;

    protected bool OnCoolDown() => Time.time < lastTimeUsed + coolDown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float coolDownReduction) => lastTimeUsed = lastTimeUsed + coolDownReduction;

    public void ResetCoolDown() => lastTimeUsed = Time.time;
}
