using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            String enemyHealthPercentage = "N/A";
        
            if(fighter.GetTargetHealth() != null)
            {
                enemyHealthPercentage = fighter.GetTargetHealth().GetPercentageHealth().ToString();
            }
           
            GetComponent<Text>().text = String.Format("{0:0}%", enemyHealthPercentage);
        }
    }

}