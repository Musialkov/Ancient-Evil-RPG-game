using System;
using System.Collections;   
using System.Collections.Generic;
using Musialkov.Inventories;
using Musialkov.Saving;
using UnityEngine;
using RPG.Core;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        QuestListNotStarted notStartedQuests;

        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onUpdate;    

        private void Awake() 
        {
            notStartedQuests = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestListNotStarted>();
        }

        public List<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void AddQuest(Quest quest)
        {
            if(!HasQuest(quest))
            {
                QuestStatus newStatus = new QuestStatus(quest);
                statuses.Add(newStatus);
                CheckIfSomeObjectiveIsComplete(quest);
                if (onUpdate != null)
                {
                    onUpdate();
                }
            }
        }

        public void AddCompletedObjective(Quest quest, string objective)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest && objective != null)
                {
                    status.AddCompletedObjective(objective);
                    if(status.IsComplete()) 
                    {
                        GiveReward(quest);
                    }
                    break;
                }
            }

            if(!HasQuest(quest))
            {
                notStartedQuests.AddObjectiveToNotStartedQuest(quest, objective);
            }

            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        public void CheckIfSomeObjectiveIsComplete(Quest quest)
        {
            if(notStartedQuests.GetNotStartedQuests().ContainsKey(quest))
            {
                foreach (string objective in notStartedQuests.GetNotStartedQuests()[quest])
                {
                    AddCompletedObjective(quest, objective);
                }
            }
        }

        public bool HasQuest(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if(status.GetQuest() == quest) 
                {
                    return true;
                }
            }
            return false;
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetRewards())
            {
                bool canAddItem = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if(!canAddItem)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        public bool? Evaluate(PredicateEnum predicate, string[] parameters)
        {
            switch (predicate)
            {
                case PredicateEnum.HasQuest:
                return HasQuest(Quest.GetByName(parameters[0]));
                case PredicateEnum.CompletedQuest:
                return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            }
            return null;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public object CaptureState()
        {
            List<object> stateList = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                stateList.Add(status.CaptureState());
            }
            return stateList;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = (List<object>)state;
            if (stateList == null) return;

            statuses.Clear();
            foreach (object stateObject in stateList)
            {
                statuses.Add(new QuestStatus(stateObject));
            }

            onUpdate();
        }
    }
}