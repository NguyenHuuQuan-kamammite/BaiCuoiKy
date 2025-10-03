using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material item", fileName = "Material data-")]
public class ItemDataSO : ScriptableObject
{
    [Header("Merchant Detail")]
    [Range(0,10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;
    [Header("Drop Detail")]
    [Range(0, 1000)]
    public int itemRarity = 100;
    [Range(0, 100)]
    public float dropChance;
    [Range(0, 100)]
    public float maxDropChance = 65f;


    [Header("Craft Detail")]
    public Inventory_Item[] craftRecipe;

    public string itemName;
    public Sprite itemIcon;
    public Item_Type itemType;

    public int maxStackSize = 1;
    [Header("Item Effects")]
    public ItemEffectDataSO itemEffects;

    private void OnValidate()
    {
        dropChance = GetDropChance();
    }
    public float GetDropChance()
    { 
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity +1 )/maxRarity * 100;
        return Mathf.Min(chance, maxDropChance);
    }

}
