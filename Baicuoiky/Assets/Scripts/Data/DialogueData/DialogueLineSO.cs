using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue Line Info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text Option")]
    [TextArea] public string[] textLine;

    [Header("Answer Setup")]
    public bool playerCanAnswer;//should be true if the player is the one responding
    public DialogueLineSO[] answerLine;


    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}
