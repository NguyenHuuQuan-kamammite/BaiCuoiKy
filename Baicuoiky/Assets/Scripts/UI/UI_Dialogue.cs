using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] private Image speakerPotrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI dialogueChoices;

    [Space]
    [SerializeField] private float textSpeed = 0.1f;
    private string fullTextToShow;
    private Coroutine typeTextCo;
    public void PlayDialogueLine(DialogueLineSO line)
    {
        speakerPotrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        fullTextToShow = line.GetRandomLine();
        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));

    }


    public void DialogueInteraction()
    {
       if (typeTextCo != null && dialogueText.text.Length > 5)
        {
            CompleteTyping();
            return;
        }

    }

    private void CompleteTyping()
    {
        if (typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow;
        }
    }
    private IEnumerator TypeTextCo(string text)
    {       
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
