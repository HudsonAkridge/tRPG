using SGoap;
using UnityEngine;

public class AttackAction : BasicAction
{
    //1 second CD time
    public override float CooldownTime => 1;

    private string _rootObjectName;
    public override void Start()
    {
        base.Start();
     
        _rootObjectName = RootTransformObject.name ?? "Unknown";
    }

    public override EActionStatus Perform()
    {
        Debug.Log($"{_rootObjectName} is Attacking!");
        return EActionStatus.Success;
    }
}
