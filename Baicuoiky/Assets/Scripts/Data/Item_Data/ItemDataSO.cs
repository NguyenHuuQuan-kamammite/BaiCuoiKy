using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data-")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public Item_Type itemType;

    public int maxStackSize = 1;
    [Header("Item Effects")]
    public ItemEffectDataSO itemEffects;
    [Header("Craft Detail")]
    public Inventory_Item[] craftRecipe;
}
