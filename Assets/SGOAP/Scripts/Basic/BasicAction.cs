﻿using UnityEngine;

namespace SGoap
{
    /// <summary>
    /// A very basic action that is paired with basic Agent data with it.
    /// </summary>
    public abstract class BasicAction : Action, IDataBind<AgentBasicData>
    {
        public AgentBasicData AgentData;
        public Transform RootTransformObject;
        public Animator RootAnimator;

        public virtual float StaggerTime => 0;

        public virtual void Start()
        {
            //We assume we put a GOAP_Agent > Actions > ActionName
            // set of objects in our hierarchy
            RootTransformObject = transform?.parent?.parent?.parent;
            RootAnimator = this.GetComponentInParent<Animator>();
        }

        public override bool PrePerform() => !Cooldown.Active;

        public override bool PostPerform()
        {
            Cooldown.Run(CooldownTime);
            AgentData.Cooldown.Run(StaggerTime);
            return true;
        }

        public void Bind(AgentBasicData data)
        {
            AgentData = data;
        }
    }
}