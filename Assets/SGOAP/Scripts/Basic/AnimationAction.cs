using SGoap;

namespace Assets.SGOAP.Scripts.Basic
{
    public class AnimationAction : BasicAction
    {
        public System.Action OnFirstPerform;

        public override bool PrePerform()
        {
           var isNotOnCooldown = base.PrePerform();

            if (isNotOnCooldown)
            {
                OnFirstPerform?.Invoke();
            }

            return isNotOnCooldown;
        }
    }
}
