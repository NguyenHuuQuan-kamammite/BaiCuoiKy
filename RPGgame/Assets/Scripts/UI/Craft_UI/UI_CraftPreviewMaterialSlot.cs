using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class UI_CraftPreviewMaterialSlot : MonoBehaviour
{
    [SerializeField] private Image materialcon;
    [SerializeField] private TextMeshProUGUI materialNameValue;

    public void SetUpMaterialSlot(ItemDataSO itemData, int availableAmount, int requireAmount)
    {
        materialcon.sprite = itemData.itemIcon;
        materialNameValue.text = itemData.itemName + " - " + availableAmount + " / " + requireAmount;
    }
}
