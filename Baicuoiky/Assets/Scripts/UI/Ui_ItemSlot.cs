using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Ui_ItemSlot : MonoBehaviour
{
    public Inventory_Item itemSlot { get; private set; }
    [Header("Ui Slot Setup")]
    [SerializeField] private TextMeshProUGUI itemStackSize;
    [SerializeField] private Image itemIcon;
    public void UpdateSlot(Inventory_Item item)
    {
        itemSlot = item;
        if (itemSlot == null)
        {

            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }
        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }
}
