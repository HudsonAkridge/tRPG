using Assets.SGOAP.Scripts.Basic;
using SGoap;
using UnityEngine;

public class AttackAction : AnimationAction
{
    //1 second CD time
    public override float CooldownTime => 1;

    private string _rootObjectName;
    public void Start()
    {
        _rootObjectName = OurRootTransform.name ?? "Unknown";
    }

    public override EActionStatus Perform()
    {
        Debug.Log($"{_rootObjectName} is Attacking!");
        return EActionStatus.Success;
    }
}
