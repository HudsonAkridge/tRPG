using SGoap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAction : BasicAction
{
    public Transform Target;

    public float Range = 1;
    public float MoveSpeed = 5;
    public float DistanceToTarget => Vector3.Distance(Target.position, RootTransformObject?.position ?? transform.position);
    
    public void Update()
    {
        if (DistanceToTarget <= Range)
        {
            States.SetState("InAttackRange", 1);
            RootAnimator.SetBool("IsAttacking", true);
        }
        else
        {
            States.RemoveState("InAttackRange");
            RootAnimator.SetBool("IsAttacking", false);
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
