using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        QuestStatus status;

        public void Setup (QuestStatus status)
        {
            this.status = status;
            if (status.GetQuest().GetIsMainQuest())
            {
                title.color = Color.yellow;
                progress.color = Color.yellow;             
            }
            
            title.text = status.GetQuest().GetTitle();
            progress.text = status.CompletedObjectivesNumber() + "/" + 
                status.GetQuest().GetObjectiveCount();
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }

    }
}
