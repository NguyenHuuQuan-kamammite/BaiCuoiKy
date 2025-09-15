using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration = 3f;

    public float GetEchoDuration() => timeEchoDuration;

    public override void TryToUseSkill()
    {
        if (CanUseSkill() == false)
            return;


        CreateTimeEcho();


    }
    public void CreateTimeEcho()
    {
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetUpEcho(this);
    }
}
    