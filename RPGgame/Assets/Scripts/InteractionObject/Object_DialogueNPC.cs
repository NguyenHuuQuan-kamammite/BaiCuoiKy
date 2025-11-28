using UnityEngine;

public class Object_DialogueNPC : Object_NPC, Iinteractable
{
    private Animator anim;
    [SerializeField] private DialogueLineSO firstDialogueLine;
    [SerializeField] private QuestDataSO[] quests;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetBool("isEleonore", true);
        }
        else
        {
            Debug.LogWarning("Animator component not found on " + gameObject.name);
        }
    }

    public override void Interact()
    {
        base.Interact();

        // Add null checks to prevent further errors
        if (ui != null && firstDialogueLine != null)
        {
            ui.OpenDialogueUI(firstDialogueLine, new DialogueNpcData(rewardNpc, quests));
        }
        else
        {
            Debug.LogError("UI or firstDialogueLine is null in " + gameObject.name);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // Your existing trigger logic
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (ui != null)
        {
            ui.HideAllToolTips();
        }
    }
}

