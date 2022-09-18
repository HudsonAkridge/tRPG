using Assets.SGOAP.Scripts.Basic;
using UnityEngine;

namespace SGoap
{
    public abstract class SetAnimatorPropertyBase<T> : MonoBehaviour
    {
        public AnimationAction Action;
        public T Property;

        private void OnValidate()
        {
            Action = GetComponent<AnimationAction>();
        }

        private void Awake()
        {
            Action.OnFirstPerform += OnActionPerform;
        }

        private void OnDestroy()
        {
            Action.OnFirstPerform -= OnActionPerform;
        }

        public void OnActionPerform()
        {
            SetProperty(Property, Action.AgentData.Animator);
        }

        public abstract void SetProperty(T property, Animator animator);
    }
}