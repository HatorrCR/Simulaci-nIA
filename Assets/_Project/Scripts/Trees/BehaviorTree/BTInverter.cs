// BTInverter.cs
namespace IAEngine.BehaviorTree
{
    public class BTInverter : BTNode
    {
        private readonly BTNode child;

        public BTInverter(BTNode child)
        {
            this.child = child;
        }

        public override BTStatus Tick()
        {
            var result = child.Tick();

            return result switch
            {
                BTStatus.Success => BTStatus.Failure,
                BTStatus.Failure => BTStatus.Success,
                _ => BTStatus.Running
            };
        }
    }
}
