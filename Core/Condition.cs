﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition 
    {
        [SerializeField] Disjunction[] and;

        public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
            {
                if(!dis.CheckCondition(evaluators)) return false;
            }
            return true;
        }

        [System.Serializable]
        public class Disjunction
        {
            [SerializeField] Predicate[] or;

            public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
            {
               foreach (Predicate pred in or)
               {
                   if(pred.CheckCondition(evaluators)) return true;
               }
               return false;
            }
        }

        [System.Serializable]
        public class Predicate
        {
            [SerializeField] PredicateEnum predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate = false;

            public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null) continue;

                    if (result == negate) return false;
                }
                return true;
            }
        }
    }
}
