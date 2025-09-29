using UnityEngine;

public class Object_BlackSmith : Object_NPC, Iinteractable
{
    private Animator anim;
    public void Interact()
    {
        Debug.Log("Open BlackSmith Shop");
        
    }
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsBlackSmith", true);
    }

}

