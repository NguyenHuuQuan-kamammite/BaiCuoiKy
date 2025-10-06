using UnityEngine;

public class UI : MonoBehaviour
{
    #region UI Components
    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatsToolTip statsToolTip { get; private set; }
    public UI_SkillTree skillTreeUI { get; private set; }
    public Ui_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI {  get; private set; }
    public UI_InGame inGameUI { get; private set; }
    #endregion
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
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>( true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    private void Start()
    {
        skillTreeUI.UnlockDefaulSkills();
    }
    public void SwitchOffAllToolTips()
    {
        skillToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
        statsToolTip.ShowToolTip(false, null);
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
