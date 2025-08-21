
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;
    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;


    [Header("Skill Details")]
    public Skill_DataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string lockColorHex = "#9F9292";
    private Color lastColor;
    
    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
        UpdateIconColor(GetColorByHex(lockColorHex));
    }
    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockColorHex));
        
        skillTree.AddSkillPoint(skillData.cost);
        connectHandler.UnlockConnectionImage(false);
    }
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoint(skillData.cost);
        LockConflictNode();
        connectHandler.UnlockConnectionImage(true);
    }
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
        {
            return false;
        }
        if (skillTree.EnoughSkillPoints(skillData.cost) == false)
        {
            Debug.Log("Not enough skill points to unlock " + skillName);
            return false;
        }
       
        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
            {
                Debug.Log("Cannot unlock " + skillName + " because " + node.skillName + " is not unlocked.");
                return false;
            }
        }
        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
            {
                return false;
            }
        }
        return true;
    }
    private void LockConflictNode()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
           
        }
    }
    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(true, rect, this);
        if (isUnlocked || isLocked)
            return;
        
        Color color = Color.white * .9f; color.a = 1f; // Semi-transparent color
        UpdateIconColor(color);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
        }
        else if (isLocked)
        {
           ui.skillToolTip.LockedSkillEffect();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        ui.skillToolTip.ShowToolTip(false, rect);
        if (isUnlocked || isLocked)
            return;
        
        UpdateIconColor(lastColor);
        
    }
    private Color GetColorByHex(string hex)
    {
       if (ColorUtility.TryParseHtmlString(hex, out Color color))
        return color;

        Debug.LogWarning("Invalid hex color: " + hex);
        return Color.white; // fallback


    }
    private void OnValidate()
{
    if (skillData == null)
        return;

    skillName = skillData.skillName;
    skillIcon.sprite = skillData.icon;
    skillCost = skillData.cost;
    gameObject.name = "UI_TreeNode - " + skillData.skillName;
}
}
