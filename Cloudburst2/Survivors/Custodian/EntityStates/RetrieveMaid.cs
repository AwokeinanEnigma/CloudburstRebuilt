using EntityStates;
using Cloudburst.Cores;
using Cloudburst.Custodian.Components;

namespace Cloudburst.CEntityStates.Custodian
{
    class RetrieveMaid : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                gameObject.GetComponent<MAIDManager>().RetrieveMAIDAuthority();
                //skillLocator.special.SetPropertyValue("cooldownRemaining", (Single)3);
                //skillLocator.special.cooldownRemaining
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}