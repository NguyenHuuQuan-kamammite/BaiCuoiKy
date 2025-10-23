using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;


    [SerializeField] private GameObject[] additionalObjects;
 
    private QuestDataSO previewQuest;


    public void SetupQuestPreviw(QuestDataSO questDataSO)
    {
        EnableAdditonalObjects(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal;
        

        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(questDataSO.rewardItems[i]);
        }
    }

    public void MakeQuestPreviwEmpty()
    {
        questName.text = "";
        questDescription.text = "";
        EnableAdditonalObjects(false);
        EnableQuestRewardObjects(false);
    }
    private void EnableAdditonalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
            obj.SetActive(enable);
    }

    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in questReward)
            obj.gameObject.SetActive(enable);
    }
}
