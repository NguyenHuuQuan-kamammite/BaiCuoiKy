using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class Skill_DataSo : ScriptableObject
{
    public int cost;
    public Skill_Type skillType;
    public SkillUnlock_Type unlockType;
    [Header("Skill Information")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
   
}
