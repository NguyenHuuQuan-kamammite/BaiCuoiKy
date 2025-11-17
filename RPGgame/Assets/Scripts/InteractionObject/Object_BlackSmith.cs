using UnityEngine;

public class Object_BlackSmith : Object_NPC, Iinteractable
{
    private Animator anim;
    [SerializeField] private DialogueLineSO firstDialogueLine;
    [SerializeField] private QuestDataSO[] quests;
    private Inventory_Player inventory;
    private Inventory_Storage storage;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();

        storage = GetComponent<Inventory_Storage>();
        anim.SetBool("IsBlackSmith", true);
    }
    public override void Interact()
    {
        base.Interact();
        ui.OpenDialogueUI(firstDialogueLine, new DialogueNpcData(rewardNpc, quests));
        ui.storageUI.SetUpStorage(storage);
        ui.craftUI.SetUpCraftUI(storage);
        //ui.OpenStorageUI(true);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllToolTips();
        ui.OpenStorageUI(false);
       
    }
}