using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_ItemSlot : MonoBehaviour, IPointerDownHandler
{

    
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;
    [Header("Ui Slot Setup")]
    [SerializeField] private TextMeshProUGUI itemStackSize;
    [SerializeField] private Image itemIcon;
    protected void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
        

    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;
        inventory.TryEquipItem(itemInSlot);
    }
    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;
        if (itemInSlot == null)
        {

            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }
        Color color = Color.white; color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
    }

    
}
