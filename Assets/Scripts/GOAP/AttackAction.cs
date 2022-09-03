using SGoap;
using UnityEngine;

public class AttackAction : BasicAction
{
    //1 second CD time
    public override float CooldownTime => 1;

    private string _rootObjectName;
    public void Start()
    {
        //We assume we put a GOAP_Agent > Actions > ActionName
        // set of objects in our hierarchy
        _rootObjectName= transform?.parent?.parent?.parent?.name ?? "Unknown";
    }

    public override EActionStatus Perform()
    {
        Debug.Log($"{_rootObjectName} is Attacking!");
        return EActionStatus.Success;
    }
}
