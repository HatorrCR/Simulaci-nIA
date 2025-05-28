// BTRepeater.cs
namespace IAEngine.BehaviorTree
{
    public class BTRepeater : BTNode
    {
        private readonly BTNode child;
        private readonly int repeatCount;
        private int currentCount = 0;

        public BTRepeater(BTNode child, int repeatCount = -1)
        {
            this.child = child;
            this.repeatCount = repeatCount;
        }

        public override BTStatus Tick()
        {
            if (repeatCount != -1 && currentCount >= repeatCount)
                return BTStatus.Success;

            var result = child.Tick();
            if (result != BTStatus.Running)
                currentCount++;

            return BTStatus.Running;
        }
    }
}
