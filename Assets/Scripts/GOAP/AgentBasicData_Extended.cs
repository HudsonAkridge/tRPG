using UnityEngine;

namespace SGoap
{
    public partial class AgentBasicData
    {
        private Transform _parentTransform;

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
    }
}
