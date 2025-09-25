using UnityEngine;
using UnityEngine.EventSystems;
public class UI_EquipSlot : Ui_ItemSlot
{
    public Item_Type slotType;
    void OnValidate()
    {
        gameObject.name = "Ui_EquipSlot_" + slotType.ToString();

    }
    override public void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;
        inventory.UnequipItem(itemInSlot);
    }
}

