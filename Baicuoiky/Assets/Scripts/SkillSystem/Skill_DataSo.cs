using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class Skill_DataSo : ScriptableObject
{
    public int cost;
    [Header("Skill Information")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
   
}
