using SGoap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAction : BasicAction
{
    public Transform Target;

    public float Range = 1;
    public float MoveSpeed = 2;

    private Transform _rootObject;
    public void Start()
    {
        //We assume we put a GOAP_Agent > Actions > ActionName
        // set of objects in our hierarchy
        _rootObject = transform?.parent?.parent?.parent;
    }

    public override EActionStatus Perform()
    {
        AgentData.Target = Target;
        var distanceToTarget = Vector3.Distance(Target.position, _rootObject?.position ?? transform.position);
        if (distanceToTarget <= Range)
        {
            return EActionStatus.Success;
        }

        var directionToTarget = (Target.position - _rootObject?.position ?? transform.position).normalized;
        AgentData.ParentPosition += directionToTarget * Time.deltaTime * MoveSpeed;

        return EActionStatus.Running;
    }
}
