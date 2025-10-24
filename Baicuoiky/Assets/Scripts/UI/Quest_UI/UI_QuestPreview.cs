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


    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        EnableAdditonalObjects(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal;
        

        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            Inventory_Item rewardItem = new Inventory_Item(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize = questDataSO.rewardItems[i].stackSize;



            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(rewardItem);
        }

    }

    public void MakeQuestPreviewEmpty()
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
