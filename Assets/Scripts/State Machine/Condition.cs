using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.StateMachine
{
    [System.Serializable]
    public class Condition
    {   
        [SerializeField] List<Disjunction> and = new List<Disjunction>();

        public Condition Clone()
        {
            Condition clone = new Condition();
            clone.and = and.ConvertAll((disjunction) => disjunction.Clone());
            return clone;
        }

        public bool Check(StateController controller, State state)
        {
            if(and.Count == 0) return false;

            foreach(var disjunction in and)
            {
                if(!disjunction.Check(controller, state))
                {
                    return false;
                }
            }

            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField] List<Predicate> or = new List<Predicate>();

            public Disjunction Clone()
            {
                Disjunction clone = new Disjunction();
                clone.or = or.ConvertAll((predicate) => predicate.Clone());
                return clone;
            }

            public bool Check(StateController controller, State state)
            {
                foreach(var predicate in or)
                {
                    if(predicate.Check(controller, state))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] StateCondition predicate;
            [SerializeField] bool negate = false;

            public Predicate Clone()
            {
                Predicate clone = new Predicate();
                clone.predicate = predicate.Clone();
                clone.negate = negate;
                return clone;
            }

            public bool Check(StateController controller, State state)
            {
                bool result = predicate.Check(controller, state);

                if(result == negate)
                {
                    return false;
                }

                return true;
            }
        }
    }   
}
