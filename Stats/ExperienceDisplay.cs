using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] RectTransform foreground = null;

        Experience experience;
        BaseStats baseStats;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            SetExperienceBar();
        }

        private void SetExperienceBar()
        {
            float xp = experience.GetExperiencePoints() - baseStats.GetPointsForCurrentLevel();
            if(xp < 0) xp = experience.GetExperiencePoints();

            float xpNeeded = baseStats.GetPointsForNextLevel() - baseStats.GetPointsForCurrentLevel();

            Vector3 newScale = new Vector3(xp/xpNeeded, 1, 1);
            foreground.localScale = newScale;
        }
    }
}
