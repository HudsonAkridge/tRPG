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
            agentBasicData.RootAIMetadata = GetComponentInParent<AIMetadata>();
            agentBasicData.RootTransformObject = agentBasicData.RootAIMetadata?.transform;
            agentBasicData.Animator ??= GetComponentInParent<Animator>();
            agentBasicData.AStarSeeker = GetComponentInParent<Seeker>();

            return agentBasicData;
        }
    }
}
