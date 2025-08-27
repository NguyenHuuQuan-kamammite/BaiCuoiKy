using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
       if(Unlocked(SkillUnlock_Type.Dash_CloneOnStart) ||  Unlocked(SkillUnlock_Type.Dash_CloneOnStartAndArrival))
       {
           CreateClone();
       }
      if(Unlocked(SkillUnlock_Type.Dash_ShardOnStart) ||  Unlocked(SkillUnlock_Type.Dash_ShardOnStartAndArrival))
       {
           CreateShard();
       } 
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUnlock_Type.Dash_CloneOnStartAndArrival))
        {
            CreateClone();
        }
        if (Unlocked(SkillUnlock_Type.Dash_ShardOnStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        Debug.Log("Create time shard");
    }
    private void CreateClone()
    {
        Debug.Log("Create clone");
    }
}
