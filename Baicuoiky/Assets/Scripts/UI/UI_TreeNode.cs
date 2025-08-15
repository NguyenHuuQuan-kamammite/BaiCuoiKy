
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Skill_DataSo skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockColorHex = "9F9292";
    private Color lastColor;
    public bool isUnlocked;
    public bool isLocked;

   private void OnValidate()
{
    if (skillData == null)
        return;

    skillName = skillData.skillName;
    skillIcon.sprite = skillData.icon;
    gameObject.name = "UI_TreeNode - " + skillData.skillName;
}

    private void Awake()
    {
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
        if (isUnlocked == false)
        {
            UpdateIconColor(lastColor);
        }
    }
    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
        
       
    }
}
