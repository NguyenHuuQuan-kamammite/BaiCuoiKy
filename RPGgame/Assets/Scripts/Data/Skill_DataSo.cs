using UnityEngine;
using System;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class Skill_DataSo : ScriptableObject
{
   
    [Header("Skill Information")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
[Header("Unlock and Upgrade")]
    public int cost;
    public bool unlockByDefault;
    public Skill_Type skillType;
    public UpgradeData upgradeData;
   
}
[Serializable]
public class UpgradeData
{
    public SkillUnlock_Type upgradeType;
    public float cooldown;
    public DamageScaleData damageScale;
}
    