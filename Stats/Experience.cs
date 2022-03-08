using System;
using Musialkov.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        
        public event Action onExperienceGained;

        public float GetExperiencePoints()
        {
            return experiencePoints;
        }
        
        public void GainExperience(float exp)
        {
            experiencePoints += exp;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float) state;
        }
    }
}
