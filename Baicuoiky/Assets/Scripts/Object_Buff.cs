using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{   
    private SpriteRenderer sr;
    [Header("Buff Settings")]
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private bool canBeUsed = true;

    [Header("Floating Settings")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsed == false)
            return;
        StartCoroutine(BuffCo(buffDuration));
    }
    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.yellow;
        Debug.Log("Buff activated for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log("Buff is over.");
        Destroy(gameObject);
    }
}
