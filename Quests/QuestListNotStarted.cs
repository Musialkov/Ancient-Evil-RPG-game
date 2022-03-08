using System.Collections;
using System.Collections.Generic;
using Musialkov.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestListNotStarted : MonoBehaviour, ISaveable
    {
        public Dictionary<Quest, List<string>> notStartedQuests = new Dictionary<Quest, List<string>>();

        public void AddObjectiveToNotStartedQuest(Quest quest, string objective)
        {
            if (!notStartedQuests.ContainsKey(quest))
            {
                notStartedQuests.Add(quest, new List<string>());
            }
            notStartedQuests[quest].Add(objective);
        }

        public Dictionary<Quest, List<string>> GetNotStartedQuests()
        {
            return notStartedQuests;
        }

        public object CaptureState()
        {
            var state = new Dictionary<string, List<string>>();
            foreach(var quest in notStartedQuests)
            {
                state[quest.Key.GetTitle()] = notStartedQuests[quest.Key];
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, List<string>> restoredList = (Dictionary<string, List<string>>) state;
            notStartedQuests = new Dictionary<Quest, List<string>>();
            foreach(var questName in restoredList)
            {
                notStartedQuests[Quest.GetByName(questName.Key)] = restoredList[questName.Key];
            }
        }
    }
}