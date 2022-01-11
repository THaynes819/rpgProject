using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Dialogue;
using RPG.Stats;
using UnityEngine;

namespace GameDevTV.Utils
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        Disjunction[] and;

        public bool Check (IEnumerable<IPredicateEvaluator> evaluators)
        {

            foreach (Disjunction disjunction in and)
            {
                if ((bool)!disjunction.Check (evaluators))
                {
                    return false;
                }
            }

            return true;
        }

        [System.Serializable]
        public class Disjunction
        {
            [SerializeField]
            Predicate[] or;
            public bool? Check (IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if ((bool)pred.Check (evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [System.Serializable]
        public class Predicate
        {
            [SerializeField] Predicates predicate;
            [SerializeField] string[] paremeters;
            [SerializeField] RequiredAttribute[] requiredAttributes;
            [SerializeField] bool negate = false;

            public bool? Check (IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate (predicate, paremeters, requiredAttributes);

                    if (result == negate) return false;
                }

                return true;
            }
        }
    }

}