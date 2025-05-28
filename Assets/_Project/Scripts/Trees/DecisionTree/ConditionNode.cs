using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

namespace IAEngine.DecisionTree
{
    public class ConditionNode : DecisionNode
    {
        public Func<bool> condition;
        public DecisionNode trueNode;
        public DecisionNode falseNode;

        public override DecisionNode MakeDecision()
        {
            return condition != null && condition() ? trueNode : falseNode;
        }
    }
}
