using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatsToolTip statsToolTip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public Ui_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    private bool skillTreeEnabled;
    private bool inventoryEnabled;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statsToolTip = GetComponentInChildren<UI_StatsToolTip>();
        inventoryUI = GetComponentInChildren<Ui_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    public void ToggleSkillTree()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
   }

    public void ToggleInventory()
    {
        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        statsToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
    }
}
