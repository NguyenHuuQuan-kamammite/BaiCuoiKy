using System.Collections;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Dialogue : MonoBehaviour
{
    private UI ui;


    [SerializeField] private Image speakerPotrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI[] dialogueChoicesText;

    [Space]
    [SerializeField] private float textSpeed = 0.1f;
    private string fullTextToShow;
    private Coroutine typeTextCo;


    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoice;
    private int selectedChoiceIndex;
    private bool waitingToConfirm;
    private bool canInteract;





    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }
   
    public void PlayDialogueLine(DialogueLineSO line)
    {
        currentLine = line;
        currentChoices = line.choiceLines;
        canInteract = false;
        HideAllChoices();


        speakerPotrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        fullTextToShow = line.GetRandomLine();
        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));
        StartCoroutine(EnableInteractionCo());
    }
    private void HandleNextAction()
    {
        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;
            case DialogueActionType.PlayerMakeChoice:
                if (selectedChoice == null)
                {

                    selectedChoiceIndex = 0;
                    ShowChoices();
                }
                break;
        }
    }

    public void DialogueInteraction()
    {
        if (canInteract == false)
            return;
        if (typeTextCo != null)
        {
            CompleteTyping();
            waitingToConfirm = true;

            return;
        }

        if (waitingToConfirm )//|| selectedChoice != null)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }

    }

    private void CompleteTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
            typeTextCo = null;
        }
    }
    private void ShowChoices()
    {
        for (int i = 0; i < dialogueChoicesText.Length; i++)
        {
            if (i < currentChoices.Length)
            {
                DialogueLineSO choice = currentChoices[i];
                string choiceText = choice.GetFirstLine();

                dialogueChoicesText[i].gameObject.SetActive(true);
              
                   
            }
            else
            {
                dialogueChoicesText[i].gameObject.SetActive(false);
            }

        }

        selectedChoice = currentChoices[selectedChoiceIndex];
    }
    public void NavigateChoice(int direction)
    {
        if (currentChoices == null || currentChoices.Length <= 1)
            return;

        selectedChoiceIndex = selectedChoiceIndex + direction;
        selectedChoiceIndex = Mathf.Clamp(selectedChoiceIndex, 0, currentChoices.Length - 1);
        ShowChoices();
    }


    private void HideAllChoices()
    {
        foreach (var obj in dialogueChoicesText)
            obj.gameObject.SetActive(false);
    }


    private IEnumerator TypeTextCo(string text)
    {       
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        waitingToConfirm = true;
        typeTextCo = null;
    }
    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }
}
