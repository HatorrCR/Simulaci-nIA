using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IAEngine.DecisionTree
{
    public abstract class DecisionNode
    {
        public abstract DecisionNode MakeDecision();
    }
}