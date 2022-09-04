using Pathfinding;
using UnityEngine;

namespace SGoap
{
    //This is a custom class used to extend the BasicAction template that comes with the SGOAP library
    public abstract partial class BasicAction
    {
        public Transform RootTransformObject;
        public AIMetadata RootAIMetadata;
        public Animator RootAnimator;
        public Seeker PathfinderSeeker;



        public virtual void Start()
        {
            RootAIMetadata = GetComponentInParent<AIMetadata>();
            RootTransformObject = RootAIMetadata.transform;
            RootAnimator = GetComponentInParent<Animator>();
            PathfinderSeeker = GetComponentInParent<Seeker>();
        }
    }
}
