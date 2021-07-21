﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
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
                if (!disjunction.Check (evaluators))
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
            public bool Check (IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check (evaluators))
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
            [SerializeField] bool negate = false;

            public bool Check (IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate (predicate, paremeters);

                    if (result == negate) return false;
                }
                return true;
            }
        }
    }

}