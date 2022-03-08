using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyAgro : MonoBehaviour
    {
        [SerializeField] Fighter[] fighters = null;
        [SerializeField] bool activateAtStart = false;

        private void Start() 
        {
            Activate(activateAtStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (var fighter in fighters)
            {
                CombatTarget target = fighter.GetComponent<CombatTarget>();
                if(target != null) 
                {
                    target.enabled = shouldActivate;
                }
                fighter.enabled = shouldActivate;
            }
        }
    }
}
