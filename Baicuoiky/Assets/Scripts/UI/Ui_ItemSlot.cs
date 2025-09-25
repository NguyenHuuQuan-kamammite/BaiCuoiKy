using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ui_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    
    public Inventory_Item itemInSlot { get; private set; }
    protected UI ui;
    protected RectTransform rect;
    protected Inventory_Player inventory;
    [Header("Ui Slot Setup")]
    [SerializeField] private TextMeshProUGUI itemStackSize;
    [SerializeField] private Image itemIcon;
    protected void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();
       

    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null || itemInSlot.itemData.itemType == Item_Type.Material)
            return;
        inventory.TryEquipItem(itemInSlot);
        if(itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null)
            return;
        ui.itemToolTip.ShowToolTip(true, rect, itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }
    
}
