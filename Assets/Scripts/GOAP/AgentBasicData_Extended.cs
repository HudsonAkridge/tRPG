using System;
using System.Collections.Generic;
using Assets.Scripts;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;
using ST = Micosmo.SensorToolkit;

namespace SGoap
{
    public partial class AgentBasicData
    {
        public ST.Sensor Sensor;
        public Transform OurRootTransform;
        public AIMetadata OurAiMetadata;
        public Seeker OurAStarSeeker;
        public GameObject CurrentTarget;
        public Vector3 OurCurrentTargetPosition
        {
            get => CurrentTarget.transform.position;
            set => CurrentTarget.transform.position = value;
        }

        protected Vector3 OurPosition
        {
            get => OurRootTransform.position;
            set => OurRootTransform.position = value;
        }

        public List<GameObject> GetAvailableTargetsByDistance([CanBeNull] Predicate<ST.Signal> optionalFilter)
        {
            Predicate<ST.Signal> defaultPredicate = signal =>
                signal.Object.layer is (int)Layers.Characters or (int)Layers.Monsters or (int)Layers.Interactables ||
                signal.Object.tag.Equals("Player", StringComparison.OrdinalIgnoreCase);

            return Sensor.GetDetectionsByDistance(optionalFilter ?? defaultPredicate);
        }
    }
}
