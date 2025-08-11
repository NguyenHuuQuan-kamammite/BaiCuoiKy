using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class Buff

{ 
    public Stats_Type type;
    public float value;
}
public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats statsToModify;
    [Header("Buff Settings")]
    [SerializeField] private Buff[] buffs;

    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private bool canBeUsed = true;

    [Header("Floating Settings")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
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
        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }
    private IEnumerator BuffCo(float duration)
    {
        canBeUsed = false;
        sr.color = Color.yellow;
        sr.enabled = false; // hides the sprite
        GetComponent<BoxCollider2D>().enabled = false;
        ApplyBuffs(true);
        yield return new WaitForSeconds(duration);
        ApplyBuffs(false);
        Destroy(gameObject);
    }
    private void ApplyBuffs(bool apply)
    {
        foreach (var buff in buffs)
        {
            if (apply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
