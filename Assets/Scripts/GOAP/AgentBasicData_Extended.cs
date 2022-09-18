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
        private Transform _parentTransform;
        public ST.Sensor Sensor;
        public Transform RootTransformObject;
        public AIMetadata RootAIMetadata;
        public Seeker AStarSeeker;

        private void InitializeParentTransform()
        {
            _parentTransform ??= Agent.GetComponentInParent<AIMetadata>().transform;
        }

        public Vector3 ParentPosition
        {
            get
            {
                InitializeParentTransform();
                return _parentTransform.position;
            }
            set
            {
                InitializeParentTransform();
                _parentTransform.position = value;
            }
        }

        public List<GameObject> GetAvailableTargetsByDistance([CanBeNull] Predicate<ST.Signal> optionalFilter)
        {
            Predicate<ST.Signal> defaultPredicate = signal =>
                signal.Object.layer is (int) Layers.Characters or (int) Layers.Monsters or (int) Layers.Interactables ||
                signal.Object.tag.Equals("Player", StringComparison.OrdinalIgnoreCase);

            return Sensor.GetDetectionsByDistance(optionalFilter ?? defaultPredicate);
        }
    }
}
