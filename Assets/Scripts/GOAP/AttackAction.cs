using SGoap;
using UnityEngine;

public class AttackAction : BasicAction
{
    //1 second CD time
    public override float CooldownTime => 1;

    public override EActionStatus Perform()
    {
        Debug.Log($"{Name} is Attacking!");
        return EActionStatus.Success;
    }
}
