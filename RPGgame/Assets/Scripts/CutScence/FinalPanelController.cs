using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
public class FinalPanelController : MonoBehaviour
{
    [Header("Text Settings")]
    public TMP_Text textMeshPro;
    [TextArea] public string fullText;

    [Header("Panel Timing (seconds)")]
    public float panelDuration = 6f;     // tổng thời gian panel hiển thị
    public float fadeInTime = 0.5f;      // thời gian fade in
    public float fadeOutTime = 0.5f;     // thời gian fade out

    [Header("Flash Settings")]
    public string wordToFlash = "terror";
    public Color flashColor = Color.yellow;
    public int flashCount = 3;
    public float flashDuration = 0.2f;

    [Header("References")]
    public CanvasGroup panelCanvasGroup;

    private bool isTyping = false;

    void OnEnable()
    {
        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 0f;

        StartCoroutine(RunFinalPanel());
    }

    IEnumerator RunFinalPanel()
    {
        int totalChars = fullText.Length;

        // ---- Fade in ----
        if (panelCanvasGroup != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / fadeInTime;
                panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }
            panelCanvasGroup.alpha = 1f;
        }

        // ---- Typewriter ----
        float remainingTimeForTypewriter = panelDuration - fadeInTime - fadeOutTime - (flashCount * flashDuration * 2);
        float typeSpeed = Mathf.Max(0.01f, remainingTimeForTypewriter / totalChars);

        isTyping = true;
        textMeshPro.text = fullText;
        textMeshPro.maxVisibleCharacters = 0;
        textMeshPro.ForceMeshUpdate();

        for (int i = 0; i < totalChars; i++)
        {
            textMeshPro.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;

        // ---- Flash word ----
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        int startIndex = textMeshPro.text.IndexOf(wordToFlash);
        if (startIndex >= 0)
        {
            int endIndex = startIndex + wordToFlash.Length - 1;
            for (int f = 0; f < flashCount; f++)
            {
                // Flash on
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible) continue;
                    int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                    Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
                    for (int j = 0; j < 4; j++)
                        vertexColors[vertexIndex + j] = flashColor;
                }
                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(flashDuration);

                // Flash off
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible) continue;
                    int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                    Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
                    for (int j = 0; j < 4; j++)
                        vertexColors[vertexIndex + j] = Color.white;
                }
                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(flashDuration);
            }
        }

        // ---- Giữ panel trước fade out ----
        float holdTime = panelDuration - fadeInTime - fadeOutTime;
        if (holdTime > 0)
            yield return new WaitForSeconds(holdTime);

        // ---- Fade out ----
        if (panelCanvasGroup != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / fadeOutTime;
                panelCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
            panelCanvasGroup.alpha = 0f;
        }

        // ---- Chuyển sang MainMenu ----
        SceneManager.LoadScene("MainMenu"); // đổi tên scene nếu khác
    }

    // Skip typewriter manually
    void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            StopAllCoroutines();
            textMeshPro.maxVisibleCharacters = textMeshPro.textInfo.characterCount;
            isTyping = false;
            // Sau khi skip xong, tự động tiếp tục Coroutine
            StartCoroutine(RunFinalPanel());
        }
    }
}
