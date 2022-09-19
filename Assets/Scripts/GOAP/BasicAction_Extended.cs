using Pathfinding;
using UnityEngine;

namespace SGoap
{
    //This is a custom class used to extend the BasicAction template that comes with the SGOAP library
    public abstract partial class BasicAction
    {
        protected Transform OurRootTransform => AgentData.OurRootTransform;

        protected Vector3 OurPosition
        {
            get => OurRootTransform.position;
            set => OurRootTransform.position = value;
        }

        protected Seeker OurAStarSeeker => AgentData.OurAStarSeeker;
    }
}
