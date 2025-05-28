using System.Collections.Generic;

namespace IAEngine.BehaviorTree
{
    public class BTSelector : BTNode
    {
        private readonly List<BTNode> children;

        public BTSelector(List<BTNode> children)
        {
            this.children = children;
        }

        public override BTStatus Tick()
        {
            foreach (BTNode child in children)
            {
                BTStatus result = child.Tick();
                if (result != BTStatus.Failure)
                    return result; // Success o Running
            }
            return BTStatus.Failure;
        }
    }
}
