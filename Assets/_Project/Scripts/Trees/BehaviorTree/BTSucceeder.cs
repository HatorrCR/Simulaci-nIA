// BTSucceeder.cs
namespace IAEngine.BehaviorTree
{
    public class BTSucceeder : BTNode
    {
        private readonly BTNode child;

        public BTSucceeder(BTNode child)
        {
            this.child = child;
        }

        public override BTStatus Tick()
        {
            child.Tick();
            return BTStatus.Success;
        }
    }
}
