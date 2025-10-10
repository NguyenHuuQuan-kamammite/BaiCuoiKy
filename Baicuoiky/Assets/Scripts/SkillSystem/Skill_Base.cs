using UnityEngine;

public class Skill_Base : MonoBehaviour
{

    public Player_SkillManager skillManager { get; private set; }
    public DamageScaleData damageScaleData { get; private set; }
    public Player player { get; private set; }
    [Header("General details")]
    [SerializeField] protected float coolDown;
    [SerializeField] protected Skill_Type skillType;
    [SerializeField] protected SkillUnlock_Type unlockType;
    private float lastTimeUsed;
    protected virtual void Awake()
    {

        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed = -coolDown; // So that the skill can be used immediately at the start
        damageScaleData = new DamageScaleData();

    }


    public virtual void TryToUseSkill()
    {
        
    }
    public void SetSkillUnlock(Skill_DataSo skillData)
    {
        UpgradeData unlock = skillData.upgradeData;
        unlockType = unlock.upgradeType;
        coolDown = unlock.cooldown;
        damageScaleData = unlock.damageScale;


        player.ui.inGameUI.GetSkillSlot(skillType).SetUpSkillSlot(skillData);
        ResetCoolDown();
    }
    public virtual bool CanUseSkill()
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


    public SkillUnlock_Type GetUpgrade() => unlockType;
    public Skill_Type GetSkillType() => skillType;

    protected bool OnCoolDown() => Time.time < lastTimeUsed + coolDown;
    public void SetSkillOnCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(coolDown); 
        lastTimeUsed = Time.time;
    }
    public void ReduceCoolDownBy(float coolDownReduction) => lastTimeUsed = lastTimeUsed + coolDownReduction;

    public void ResetCoolDown()
    {
       player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
       lastTimeUsed = Time.time - coolDown;
    }
}
