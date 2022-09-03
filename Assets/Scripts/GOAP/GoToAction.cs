using SGoap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAction : BasicAction
{
    public Transform Target;

    public float Range = 1;
    public float MoveSpeed = 2;

    public float DistanceToTarget => Vector3.Distance(Target.position, RootTransformObject?.position ?? transform.position);

    private void Update()
    {
        if (DistanceToTarget <= Range)
        {
            States.SetState("InAttackRange", 1);
        }
        else
        {
            States.RemoveState("InAttackRange");
        }
    }

    public override EActionStatus Perform()
    {
        AgentData.Target = Target;
        if (DistanceToTarget <= Range)
        {
            return EActionStatus.Success;
        }

        var directionToTarget = (Target.position - RootTransformObject?.position ?? transform.position).normalized;
        AgentData.ParentPosition += directionToTarget * Time.deltaTime * MoveSpeed;

        return EActionStatus.Running;
    }
}
