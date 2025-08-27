using UnityEngine;
using System;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class Skill_DataSo : ScriptableObject
{
    public int cost;
    public Skill_Type skillType;
    public UpgradeData upgradeData;
   
    [Header("Skill Information")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;

}
[Serializable]
public class UpgradeData
{
    public SkillUnlock_Type upgradeType;
    public float cooldown;
}
