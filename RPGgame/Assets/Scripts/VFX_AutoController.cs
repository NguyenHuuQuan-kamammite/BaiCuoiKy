using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private bool randomPosition = true;
    [SerializeField] private bool randomRotation = true;
    [Header("VFX Random RotationSettings")]
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;
    [Header("Fade Effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1f;
    [Header("VFX Random PositionSettings")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.5f;
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.5f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()

    {

        if (canFade)
        {
            StartCoroutine(FadeCo());
        }
        ApplyRandomOffset();
        ApplyRandomRotation();
        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;
        while (targetColor.a > 0f)
        {
            targetColor.a -= Time.deltaTime * fadeSpeed;
            sr.color = targetColor;
            yield return null;
        }
        sr.color = targetColor;
    }
    private void ApplyRandomOffset()
    {
        if (!randomPosition) return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(xOffset, yOffset, 0f);
    }
    private void ApplyRandomRotation()
    {
        if (!randomRotation) return;

        float randomZRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0f, 0f, randomZRotation);
    }
    
}