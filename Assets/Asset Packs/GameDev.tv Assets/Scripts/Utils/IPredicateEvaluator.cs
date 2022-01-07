using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace GameDevTV.Utils
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate (Predicates predicate, string[] parameters, RequiredAttribute[] attributes);

        RPG.Stats.Attribute[] GetRequiredAttributes();

        float GetRequiredValue();
    }
}