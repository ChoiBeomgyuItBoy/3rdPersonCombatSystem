using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class StateCondition
    {   
        [SerializeField] List<Disjunction> and = new List<Disjunction>();

        public bool Check(StateController controller, State caller)
        {
            if(and.Count == 0) return false;

            foreach(var disjunction in and)
            {
                if(!disjunction.Check(controller, caller))
                {
                    return false;
                }
            }

            return true;
        }

        public StateCondition Clone()
        {
            StateCondition clone = new StateCondition();
            clone.and = and.ConvertAll((disjunction) => disjunction.Clone());
            return clone;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] List<Predicate> or = new List<Predicate>();

            public bool Check(StateController controller, State caller)
            {
                foreach(var predicate in or)
                {
                    if(predicate.Check(controller, caller))
                    {
                        return true;
                    }
                }

                return false;
            }

            public Disjunction Clone()
            {
                Disjunction clone = new Disjunction();
                clone.or = or.ConvertAll((predicate) => predicate.Clone());
                return clone;
            }

        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] StatePredicate predicate;
            [SerializeField] bool negate = false;

            public bool Check(StateController controller, State caller)
            {
                bool result = predicate.Check(controller, caller);

                if(result == negate)
                {
                    return false;
                }

                return true;
            }

            public Predicate Clone()
            {
                Predicate clone = new Predicate();
                clone.predicate = predicate.Clone();
                clone.negate = negate;
                return clone;
            }
        }
    }   
}
