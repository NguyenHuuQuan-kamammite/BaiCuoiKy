using System;
using UnityEngine;

public class UI_Quest : MonoBehaviour, ISaveable
{
    private GameData currentGameData;

    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_QuestPreview questPreview;
    private UI_QuestSlot[] questSlots;
    public Player_QuestManager questManager { get; private set; }

    private void Awake()
    {
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);
        questManager = Player.instance.questManager;
    }
   
    public void SetupQuestUI(QuestDataSO[] questsToSetup)
    {
        foreach (var slot in questSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0; i < questsToSetup.Length; i++)
        {
            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupQuestSlot(questsToSetup[i]);
        }

        questPreview.MakeQuestPreviewEmpty();
        inventorySlots.UpdateSlots(Player.instance.inventory.itemList);

        UpdateQuestList();
    }
    public void UpdateQuestList()
    {
        foreach (var slot in questSlots)
        {
            if (slot.questInSlot == null) continue;

            if (slot.gameObject.activeSelf && CanTakeQuest(slot.questInSlot) == false)
                slot.gameObject.SetActive(false);
        }
    }

    private bool CanTakeQuest(QuestDataSO questToCheck)
    {
        if (questToCheck == null) return false;

        bool questActive = questManager.QuestIsActive(questToCheck);

        // Check both sources to ensure we have the most accurate state
        bool questCompleted =
            questManager.completedQuests.Exists(q => q.questDataSO == questToCheck) ||
            (currentGameData != null &&
             currentGameData.completedQuests.TryGetValue(questToCheck.questSaveId, out bool isCompleted) &&
             isCompleted);

        return !questActive && !questCompleted;
    }

   

    public UI_QuestPreview GetQuestPreview() => questPreview;

    public void LoadData(GameData data)
    {
        currentGameData = data;
        if (gameObject.activeInHierarchy)
        {
            UpdateQuestList();
        }
    }

    public void SaveData(ref GameData data)
    {
     
    }
}
