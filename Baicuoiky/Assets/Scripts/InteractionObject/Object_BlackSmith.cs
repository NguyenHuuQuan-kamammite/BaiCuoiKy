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
        ui.storageUI.SetUpStorage(inventory, storage);
        ui.storageUI.gameObject.SetActive(true);

    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }

}

