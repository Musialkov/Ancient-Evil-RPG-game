using System.Collections;
using System.Collections.Generic;
using Musialkov.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour, ISaveable
    {
        [SerializeField] List<Quest> quests = new List<Quest>();
        int currentIndex = 0;

        public void GiveQuest()
        {
            if(currentIndex < quests.Count)
            {
                QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
                questList.AddQuest(quests[currentIndex]);
                Debug.Log("Dałem questa " + quests[currentIndex]);
                currentIndex++;       
            }       
        }

        public object CaptureState()
        {
            return currentIndex;
        }


        public void RestoreState(object state)
        {
            currentIndex = (int) state;
        }
    }
}
