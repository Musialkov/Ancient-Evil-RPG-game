using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionClass[] progressionClass = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> progressionTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildProgressionTable();
            float[] levels = progressionTable[characterClass][stat];

            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildProgressionTable()
        {
            if(progressionTable != null) return;

            progressionTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionClass progressionClass in this.progressionClass)
            {
                var statTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statTable[progressionStat.stat] = progressionStat.levels;
                }

                progressionTable[progressionClass.characterClass] = statTable;
            }
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildProgressionTable();
            float[] levels = progressionTable[characterClass][stat];
            return levels.Length;
        }

        [System.Serializable]
        class ProgressionClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {   
            public Stat stat;
            public float[] levels;
        }
    }
}