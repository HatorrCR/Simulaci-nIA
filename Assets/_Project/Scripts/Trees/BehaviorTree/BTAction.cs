using System;

namespace IAEngine.BehaviorTree
{
    public class BTAction : BTNode
    {
        private readonly Func<BTStatus> action;

        public BTAction(Func<BTStatus> action)
        {
            this.action = action;
        }

        public override BTStatus Tick()
        {
            return action();
        }
    }
}
