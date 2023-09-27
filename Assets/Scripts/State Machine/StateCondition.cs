using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class StateCondition
    {   
        [SerializeField] List<Disjunction> and = new List<Disjunction>();

        public bool Check()
        {
            if(and.Count == 0) return false;

            foreach(var disjunction in and)
            {
                if(!disjunction.Check())
                {
                    return false;
                }
            }

            return true;
        }

        public void Bind(StateController controller, State caller)
        {
            and.ForEach((and) => and.Bind(controller, caller));
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

            public bool Check()
            {
                foreach(var predicate in or)
                {
                    if(predicate.Check())
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Bind(StateController controller, State caller)
            {
                or.ForEach((or) => or.Bind(controller, caller));
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
            public bool Check()
            {
                bool result = predicate.Check();

                if(result == negate)
                {
                    return false;
                }

                return true;
            }

            public void Bind(StateController controller, State caller)
            {
                predicate.Bind(controller, caller);
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
