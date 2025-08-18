
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;

    [SerializeField] private Skill_DataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockColorHex = "#9F9292";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;
    
    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();

        UpdateIconColor(GetColorByHex(lockColorHex));
    }
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }
    private bool CanBeUnlocked()
    {
        if (isLocked || isUnlocked)
        {
            return false;
        }
        return true;
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
        ui.skillToolTip.ShowToolTip(true, rect, skillData);
        if (isUnlocked == false)
            UpdateIconColor(Color.white * .9f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CanBeUnlocked())
        {
            Unlock();
            Debug.Log("Skill unlocked!");
        }
        else
        {
            Debug.Log("Skill cannot be unlocked.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        ui.skillToolTip.ShowToolTip(false, rect);
        if (isUnlocked == false)
        {
            UpdateIconColor(lastColor);
        }
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
    gameObject.name = "UI_TreeNode - " + skillData.skillName;
}
}
