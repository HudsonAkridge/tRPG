using Pathfinding;
using UnityEngine;
using ST = Micosmo.SensorToolkit;


namespace SGoap
{
    public partial class BasicAgent
    {
        public AgentBasicData AddExtendedData(AgentBasicData agentBasicData)
        {
            agentBasicData.Sensor = GetComponentInParent<ST.Sensor>();
            agentBasicData.OurAiMetadata = GetComponentInParent<AIMetadata>();
            agentBasicData.OurRootTransform = agentBasicData.OurAiMetadata?.transform;
            agentBasicData.Animator ??= GetComponentInParent<Animator>();
            agentBasicData.OurAStarSeeker = GetComponentInParent<Seeker>();

            return agentBasicData;
        }
    }
}
