using System.Collections.Generic;

namespace IAEngine.BehaviorTree
{
    public class BTSequence : BTNode
    {
        private readonly List<BTNode> children;

        public BTSequence(List<BTNode> children)
        {
            this.children = children;
        }

        public override BTStatus Tick()
        {
            foreach (BTNode child in children)
            {
                BTStatus result = child.Tick();
                if (result != BTStatus.Success)
                    return result; // Running o Failure
            }
            return BTStatus.Success;
        }
    }
}
