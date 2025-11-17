using UnityEngine;
using UnityEngine.Rendering;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;


    private float expandSpeed = 2;
    private float duration;
    private float slowDownPercent = .9f;

    private Vector3 targetScale;
    private bool isShrinkng;
  

    public void SetUpDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;
       

        duration = domainManager.GetDomainDuration();
        slowDownPercent = domainManager.GetSlowPercentage();
        expandSpeed = domainManager.expandSpeed;
        float maxSize = domainManager.maxDomainSize;



        targetScale = Vector3.one * maxSize;
       
        Invoke(nameof(ShrinkDomain), duration);
    }

    public void Update()
    {
        HanndleScaling();
    }

    private void HanndleScaling()
    {
        float sizeDiffrence = Mathf.Abs(transform.lossyScale.x - targetScale.x);
        bool shouldChangeSize = sizeDiffrence > .1f;

        if (shouldChangeSize)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);


        if (isShrinkng && sizeDiffrence < .1f)
        {
            TerminateDomain();
        }
    }

    private void TerminateDomain()
    {
            domainManager.ClearTargets();
            Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinkng = true;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;

        domainManager.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slowDownPercent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.StopSlowDown(); 
    }
}
