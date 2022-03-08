using System.Collections;
using System.Collections.Generic;
using Musialkov.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Mana : MonoBehaviour, ISaveable
    {
        private BaseStats baseStats;

        private float mana = -1f;

        public float ManaValue {get => mana;}

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
        }

        private void Start() 
        {
            if(mana < 0)
            {
                mana = baseStats.GetStat(Stat.Mana);
            }
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += SetMaximumMana;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= SetMaximumMana;
        }

        public void ReduceMana(float manaCost)
        {
            mana -= manaCost;
            if(mana < 0)
            {
                mana = 0;
            }
        }

        public void BoostMana(float manaBoost)
        {
            mana += manaBoost;
            if (mana > baseStats.GetStat(Stat.Mana))
            {
                mana = baseStats.GetStat(Stat.Mana);
            }
        }

        public float GetFraction()
        {
            return mana / baseStats.GetStat(Stat.Mana);
        }

        protected void SetMaximumMana()
        {
            mana = baseStats.GetStat(Stat.Mana);
        }

        public object CaptureState()
        {
            return mana;
        }

        public void RestoreState(object state)
        {
            mana = (float) state;
        }
    }
}
