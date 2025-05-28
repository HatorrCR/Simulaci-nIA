using System;

namespace IAEngine.BehaviorTree
{
    public class BTCondition : BTNode
    {
        private readonly Func<bool> condition;

        public BTCondition(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override BTStatus Tick()
        {
            return condition() ? BTStatus.Success : BTStatus.Failure;
        }
    }
}
