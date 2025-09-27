using System.Collections;
using UnityEngine;

public class Player_Vfx : Entity_VFX
{
    [Header("Time echo VFX")]
    [Range(.01f, .2f)]
    [SerializeField] private float timeEchoInterval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCo;



    public void CreateEffectOf(GameObject effect,Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }
   
    public void DoImageEchoEffect(float duration)
    {
        if (imageEchoCo != null)
        {
            StopCoroutine(imageEchoCo);
        }
        imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }
    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            CreateImageEcho();
            yield return new WaitForSeconds(timeEchoInterval);
            timer += timeEchoInterval;
        }
    }
    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
       
    }

}

