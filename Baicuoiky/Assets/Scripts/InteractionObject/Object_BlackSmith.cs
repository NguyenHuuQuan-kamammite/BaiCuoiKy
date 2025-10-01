using UnityEngine;

public class Object_BlackSmith : Object_NPC, Iinteractable
{
    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();

        storage = GetComponent<Inventory_Storage>();
        anim.SetBool("IsBlackSmith", true);
    }
    public void Interact()
    {
        ui.storageUI.SetUpStorage(storage);
        ui.craftUI.SetUpCraftUI(storage);
        ui.storageUI.gameObject.SetActive(true);
        //ui.craftUI.gameObject.SetActive(true);

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
        ui.storageUI.gameObject.SetActive(false);
        ui.SwitchOffAllToolTips();
        ui.craftUI.gameObject.SetActive(false);
    }
}