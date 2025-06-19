using UnityEngine;
using UnityEngine.Rendering;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private bool randomPosition = true;
    [SerializeField] private bool randomRotation = true;
    [Header("VFX Random PositionSettings")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.5f;
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.5f;

    private void Start()

    {
        ApplyRandomOffset();
        ApplyRandomRotation();
        if (autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
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

        float randomZRotation = Random.Range(0f, 360f);
        transform.Rotate(0f, 0f, randomZRotation);
    }
}