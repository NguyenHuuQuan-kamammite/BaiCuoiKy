using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput {  get; private set; }
    private PlayerInputSet input;
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
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScreen deathScreenUI { get; private set; }
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
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>( true);


        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    private void Start()
    {
        skillTreeUI.UnlockDefaulSkills();
    }
    public void SetupControlUI(PlayerInputSet inputSet)
    {
        input = inputSet;
        input.UI.SkillTreeUI.performed += ctx => ToggleSkillTree();
        input.UI.InventoryUI.performed += ctx => ToggleInventory();

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true; 
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;

        input.UI.OptionUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            Time.timeScale = 0;
            OpenOptionUI();
        };

    }
    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);
        input.Disable();
    }

    public void OpenOptionUI()
    {
        
        HideAllToolTips();
        StopPlayerControls(true);
        SwitchTo(optionsUI.gameObject);
    }
    public void SwitchToInGameUI()
    {
       

        HideAllToolTips();
        StopPlayerControls(false);
        SwitchTo(inGameUI.gameObject);

        skillTreeEnabled = false;
        inventoryEnabled = false;
    }
    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        objectToSwitchOn.SetActive(true);
    }
    private void StopPlayerControls(bool stopControls)

    {
        if (stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

    private void StopPlayerControlsIfNeeded()
    {
        foreach( var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;

            }
        }
        StopPlayerControls(false);
    }



    public void ToggleSkillTree()
    {

        skillTreeUI.transform.SetAsLastSibling();
        SetToolTipAbove();
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        skillToolTip.ShowToolTip(false, null);
        StopPlayerControlsIfNeeded();
   }

    public void ToggleInventory()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetToolTipAbove();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        HideAllToolTips();
        StopPlayerControlsIfNeeded();
    }
    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControls(openStorageUI);
        if(openStorageUI == false)
        {
            craftUI.gameObject.SetActive(false);
            HideAllToolTips();
        }
    }
    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(openMerchantUI);
        if(openMerchantUI == false)
            HideAllToolTips();
    }
    public void HideAllToolTips()
    {
        skillToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
        statsToolTip.ShowToolTip(false, null);
    }
    private void SetToolTipAbove()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statsToolTip.transform.SetAsLastSibling();
    }
}
