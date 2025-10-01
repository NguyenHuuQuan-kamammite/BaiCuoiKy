using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;
    [SerializeField] UI_CraftPreview craftPreview;
    [SerializeField] private Image craftIcon;
    [SerializeField] private TextMeshProUGUI craftItemName;

    public void SetUpButton(ItemDataSO craftData)
    {
        this.itemToCraft = craftData;
        craftIcon.sprite = craftData.itemIcon;
        craftItemName.text = craftData.itemName;
    }
    public void UpdateCraftPreview() => craftPreview.UpdateCraftPreview(itemToCraft);
   
}
