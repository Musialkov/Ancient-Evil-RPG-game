using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();
        private object stateObject;

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object stateObject)
        {
            QuestStatusRecord questStatusRecord = (QuestStatusRecord) stateObject;
            this.quest = Quest.GetByName(questStatusRecord.questName);
            this.completedObjectives = questStatusRecord.completedObjectives;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int CompletedObjectivesNumber()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveCompleted(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public bool IsComplete()
        {
            return quest.GetObjectiveCount() == completedObjectives.Count;
        }

        public void AddCompletedObjective(string objective)
        {
            if(quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
        }

        public object CaptureState()
        {
            QuestStatusRecord questStatusRecord = new QuestStatusRecord();
            questStatusRecord.questName = quest.name;
            questStatusRecord.completedObjectives = completedObjectives;

            return questStatusRecord;
        }
    }
}
