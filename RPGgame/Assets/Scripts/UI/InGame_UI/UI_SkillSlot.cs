using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    private Image skillIcon;
    private RectTransform rect;
    private Button button;

    private Skill_DataSo skillData;
    public Skill_Type skillType;

    [SerializeField] private Image cooldownImage;
    [SerializeField] private string inputKeyName;
    [SerializeField] private TextMeshProUGUI inputKeyText;
    [SerializeField] private GameObject conflicSlot;
    [SerializeField] private Sprite defaultEmptySprite;
    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        button = GetComponent<Button>();
        skillIcon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }
    private void OnValidate()
    {
        gameObject.name = "UI_SkillSlot -" + skillType.ToString();
    }
    public void SetUpSkillSlot(Skill_DataSo selectedSkill)


    {
        this.skillData = selectedSkill;
        Color color = Color.black; color.a = .6f;
        cooldownImage.color = color;
        skillIcon.sprite = selectedSkill.icon;
        inputKeyText.text = inputKeyName;

        if(conflicSlot != null )
        {
            conflicSlot.SetActive(false);
        }
    }
    public void StartCooldown(float cooldown)

    {
        cooldownImage.fillAmount = 1f;
        StartCoroutine(CooldownCo(cooldown));
    }


    public void ResetCooldown() => cooldownImage.fillAmount=0f;
   
    private IEnumerator CooldownCo(float duration)
    {
        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            cooldownImage.fillAmount = 1f - (timePassed / duration);
            yield return null;
        }
        cooldownImage.fillAmount = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(false, null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(skillData == null)
            return;
        ui.skillToolTip.ShowToolTip(true, rect, skillData,null);
    }
    public void ClearSkillSlot()
    {
        // Clear the skill data reference
        skillData = null;

        // Set to default empty sprite
        if (skillIcon != null)
        {
            skillIcon.sprite = defaultEmptySprite;
            Color color = skillIcon.color;
            color.a = 0.6f; // Semi-transparent to show it's empty
            skillIcon.color = color;
        }

        // Clear the input key text
        if (inputKeyText != null)
            inputKeyText.text = "";

        // Reset cooldown
        ResetCooldown();

        // Keep conflict slot hidden when clearing
        if (conflicSlot != null)
        {
            conflicSlot.SetActive(false); // Changed from true to false
        }
    }
}
