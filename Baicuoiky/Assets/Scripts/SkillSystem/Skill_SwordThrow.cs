

using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;
    [Header("Regular Sword Upgrade")]
    [Range(1f, 10f)]
    [SerializeField] private float throwPower = 5f;
    [SerializeField] private GameObject swordPrefab;

    [Header("Trajectory Prediction")]

    
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numOfDots = 20;
    [SerializeField] private float spaceBetweenDots = 0.05f;
    private float swordGravity = 3.5f;

    private Transform[] dots;
    private Vector2 confirmDirection;

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {

        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }



    private Vector2 GetThrowPower() => confirmDirection * (throwPower * 10f);
    public void ThrowSword()
    {
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);
        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetUpSword(this, GetThrowPower());
      
    }

    public void PredictTrajectory(Vector2 direction)
    {


        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }


    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = throwPower * 10f;

        // This gives us the initial velocity – the starting speed and direction of the throw
        Vector2 initialVelocity = direction * scaledThrowPower;

        // Gravity pulls the sword down over time. The longer it’s in the air, the more it drops.
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);


        // We calculate how far the sword will travel after time 't',
        // by combining the initial throw direction with the gravity pull.
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPos = transform.root.position;
        return playerPos + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction)
    {
        confirmDirection = direction;
    }

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
        {
            t.gameObject.SetActive(enable);
        }
    }


    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numOfDots];
        for (int i = 0; i < numOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }
        return newDots;
    }
}
