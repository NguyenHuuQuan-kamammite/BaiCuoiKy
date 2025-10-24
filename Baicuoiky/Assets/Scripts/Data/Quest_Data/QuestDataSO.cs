using UnityEngine;
using UnityEditor;

public enum RewardType { Merchant,BlackSmith,None}; 

[CreateAssetMenu(menuName = "RPG Setup/Quest Data/New Quest", fileName = "Quest-")]

public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    public string questName;

    [TextArea] public string description;
    [TextArea] public string questGoal;

    public string questTargetId; //enemy name, npc name.ect
    public int requiredAmount;

    [Header("Reward Type")]
    public  RewardType rewardType;
    public Inventory_Item[] rewardItems;


    private void OnValidate()
    {
# if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        questSaveId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
