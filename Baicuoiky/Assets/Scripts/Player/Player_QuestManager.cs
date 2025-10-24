using System.Collections.Generic;
using UnityEngine;

public class Player_QuestManager : MonoBehaviour
{
    public List<QuestData> activeQuests;


    public void AddProgress(string questTargetId, int amount = 1)
    {
        List<QuestData> getRewardQuests = new List<QuestData>();

        foreach (var quest in activeQuests)
        {
            if (quest.questDataSO.questTargetId != questTargetId)
                continue;

            if (quest.CanGetReward() == false)
                quest.AddQuestProgress(amount);

            if (quest.questDataSO.rewardType == RewardType.None && quest.CanGetReward())
                getRewardQuests.Add(quest);
        }

       
    }
    public void AcceptQuest(QuestDataSO questDataSO)
    {
        activeQuests.Add(new QuestData(questDataSO));
    }
    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if (questToCheck == null)
            return false;

        return activeQuests.Find(q => q.questDataSO == questToCheck) != null;
    }
}
