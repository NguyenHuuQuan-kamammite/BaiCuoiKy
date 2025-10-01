using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    private UI_CraftSlot[] craftSlots;
   private UI_CraftListButton[] craftListButton;


    private void Awake()
    {
        SetupCraftListButton();
    }
    private void SetupCraftListButton()
    {
        craftListButton = GetComponentsInChildren<UI_CraftListButton>();
        craftSlots = GetComponentsInChildren<UI_CraftSlot>();
        foreach (var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var button in craftListButton)
        {
            button.SetCraftSlot(craftSlots);
        }
    }
}
