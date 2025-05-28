using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAEngine.DecisionTree
{
    public class ActionNode : DecisionNode
    {
        public System.Action action;
        public override DecisionNode MakeDecision()
        {
            action?.Invoke();
            return this;
        }
    }
}