using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    protected Player player;
    public string effectDescription;
    public virtual bool CanBeUsed()
    {
        return true;
    }
    public virtual void ExecuteEffect()
    {
        Debug.Log("Base Item Effect Executed");
    }
    public virtual void Subcribe(Player player)
    {
        this.player = player;
    }
    public virtual void Unsubcribe()
    {
        
    }
}
