using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 10)]
        [SerializeField] int startLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffects = null;
        [SerializeField] bool shouldUseModifier = false;

        Experience experience;
        int currentLevel = 0;
        
        public event Action onLevelUp;

        private void Awake() 
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();        
        }

        private void OnEnable() 
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() 
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            float baseBonus = progression.GetStat(stat, characterClass, CalculateLevel());
            return (baseBonus + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;

            float sumOfBonus = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifierProvider in modifierProviders)
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    sumOfBonus += modifier;
                }
            }
            return sumOfBonus;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifier) return 0;

            float sumOfPercentageBonus = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifierProvider in modifierProviders)
            {
                foreach (float modifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    sumOfPercentageBonus += modifier;
                }
            }
            return sumOfPercentageBonus;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffects();
                onLevelUp();
            }
        }

        private void LevelUpEffects()
        {
            Instantiate(levelUpEffects, transform);
        }

        public int CalculateLevel()
        {
            if (experience == null) return startLevel;

            float currentExperience = experience.GetExperiencePoints();
            int currentLevel = 1;
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int i = 0; i < maxLevel; i++)
            {
                float experienceNeedToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, currentLevel);
                if (currentExperience >= experienceNeedToLevelUp)
                {
                    currentLevel++;
                }
            }
            return currentLevel;
        }

        public float GetPointsForCurrentLevel()
        {
            return progression.GetStat(Stat.ExperienceToLevelUp, characterClass, CalculateLevel());
        }

        public float GetPointsForNextLevel()
        {
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            int currentLevel = CalculateLevel();
            if(currentLevel + 1 < maxLevel)
            {
                return progression.GetStat(Stat.ExperienceToLevelUp, characterClass, currentLevel + 1);
            }

            return Int16.MaxValue;
        }
    }
}
