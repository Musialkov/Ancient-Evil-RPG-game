using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectivesContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectivePrefabUncheck;

        public void Setup(QuestStatus status)
        {
            title.text = status.GetQuest().GetTitle();

            foreach (var objective in status.GetQuest().GetObjectives())
            {
                GameObject prefabVariant = objectivePrefabUncheck;
                if(status.IsObjectiveCompleted(objective.reference))
                {
                    prefabVariant = objectivePrefab;
                }
                GameObject obj = Instantiate(prefabVariant, objectivesContainer);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = objective.description;
            }          
        }
    }
}
