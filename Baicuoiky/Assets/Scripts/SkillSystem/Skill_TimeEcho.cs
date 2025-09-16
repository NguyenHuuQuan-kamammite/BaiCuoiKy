using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration = 3f;

    [Header("Attack Upgrade")]
    [SerializeField] private int maxAttack = 3;
    [SerializeField] private float duplicateChance = .3f;

    public float GetDuplicateChance()
    {
        if (unlockType != SkillUnlock_Type.TimeEcho_ChanceToDuplicate)
            return 0;
        return duplicateChance;
    }
    public int GetMaxAttack()
    {
        if (unlockType == SkillUnlock_Type.TimeEcho_SingleAttack || unlockType == SkillUnlock_Type.TimeEcho_ChanceToDuplicate)
            return 1;
        if (unlockType == SkillUnlock_Type.TimeEcho_MultiAttack)
            return maxAttack;
        return 0;
    }
    public float GetEchoDuration() => timeEchoDuration;

    public override void TryToUseSkill()
    {
        if (CanUseSkill() == false)
            return;


        CreateTimeEcho();


    }
    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position = targetPosition ?? transform.position;
        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetUpEcho(this);
    }
}
    