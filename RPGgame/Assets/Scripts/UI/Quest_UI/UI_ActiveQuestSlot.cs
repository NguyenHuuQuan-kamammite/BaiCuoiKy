using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_ActiveQuestSlot : MonoBehaviour
{
    private QuestData questInSlot;
    private UI_ActiveQuestPreview questPreview;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] questRewardPreviw;

    public void SetupActiveQuestSlot(QuestData questToSetup)
    {
        questPreview = transform.root.GetComponentInChildren<UI_ActiveQuestPreview>();
        questInSlot = questToSetup;

        questName.text = questToSetup.questDataSO.questName;

        Inventory_Item[] reward = questToSetup.questDataSO.rewardItems;

        foreach (var previewIcon in questRewardPreviw)
        {
            previewIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] == null) continue;
            Image preview = questRewardPreviw[i];

            preview.gameObject.SetActive(true);
            preview.sprite = reward[i].itemData.itemIcon;
            preview.GetComponentInChildren<TextMeshProUGUI>().text = reward[i].stackSize.ToString();
        }
    }

    public void SetupPreviewBTN()
    {
        questPreview.SetupQuestPreviw(questInSlot);
    }
}
