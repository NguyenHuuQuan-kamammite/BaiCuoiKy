using UnityEngine;

public class Object_NPC : MonoBehaviour,Iinteractable
{
    protected Transform player;
    protected UI ui;
    protected Player_QuestManager questManager;

    [Header("Quest Info")]
    [SerializeField] private string npcTargetQuestId;
    [SerializeField] private RewardType rewardNpc;

    [Space]

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactTooltip;
    private bool FacingRight = true;

     [Header("Floating ToolTip")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;


    protected virtual void Awake()
    {

        ui = FindFirstObjectByType<UI>();
        startPosition = interactTooltip.transform.position;
        interactTooltip.SetActive(false);

    }

    protected virtual void Start()
    {
        questManager = Player.instance.questManager;
    }



    private void HandleToolTipFloat()
    {
        if (interactTooltip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactTooltip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }


    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }
    private void HandleNpcFlip()
    {
        if (player == null || npc == null) return;
        if (player.position.x < npc.position.x && FacingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            FacingRight = false;
        }
        else if (player.position.x > npc.position.x && !FacingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            FacingRight = true;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactTooltip.SetActive(true);
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactTooltip.SetActive(false);
    }

    public virtual void Interact()
    {

        questManager.AddProgress(npcTargetQuestId);
        questManager.TryGiveRewardFrom(rewardNpc);
    }
}
