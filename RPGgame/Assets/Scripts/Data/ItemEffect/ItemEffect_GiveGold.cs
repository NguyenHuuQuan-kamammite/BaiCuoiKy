using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grand Gold", fileName = "Item Effect data- Grand Gold")]
public class ItemEffect_GiveGold : ItemEffectDataSO
{
    [Header("Gold Settings")]
    [SerializeField] private int goldAmount = 100;

    
    public override void ExecuteEffect()
    {
        Inventory_Player playerInventory = FindFirstObjectByType<Inventory_Player>();
        if (playerInventory != null)
        {
            playerInventory.AddGold(goldAmount);
        }
        else
        {
            Debug.LogError("Player inventory is null! Cannot add gold.");
        }
    }
}
