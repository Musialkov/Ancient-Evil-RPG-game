﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    using System;
    using Musialkov.Inventories;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject 
    {
        [SerializeField] bool isMainQuest = false;
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();
        
        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        public string GetTitle()
        {
            return name;
        }

        public bool GetIsMainQuest()
        {
            return isMainQuest;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach (var objective in objectives)
            {
                if(objective.reference == objectiveRef)
                {
                    return true;
                }
            }
            return false;
        }

        public static Quest GetByName(string name)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if(quest.name == name)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}
