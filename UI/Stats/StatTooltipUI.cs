using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace RPG.UI.Stats
{
    public class StatTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;

        Health health;
        Mana mana;
        BaseStats baseStats;
        Experience experience;

        private void Awake() 
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            health = player.GetComponent<Health>();
            mana = player.GetComponent<Mana>();
            baseStats = player.GetComponent<BaseStats>();
            experience = player.GetComponent<Experience>();
        }

        public void Setup(Stat stat)
        {
            switch(stat)
            {
                case Stat.Health:
                titleText.text = "Health";
                bodyText.text = health.GetHealth() + " / " + baseStats.GetStat(Stat.Health); 
                break;

                case Stat.Mana:
                titleText.text = "Mana";
                bodyText.text = mana.ManaValue + " / " + baseStats.GetStat(Stat.Mana);
                break;

                case Stat.Experience:
                titleText.text = "Experience";
                bodyText.text = experience.GetExperiencePoints() + " / " + baseStats.GetPointsForNextLevel();
                if(baseStats.CalculateLevel() == 1)
                {
                    bodyText.text = experience.GetExperiencePoints() + " / " + baseStats.GetPointsForCurrentLevel();
                }              
                break;
            }
        }
    }
}
