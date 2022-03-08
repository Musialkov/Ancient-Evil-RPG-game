using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using System.Linq;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        QuestList questList = null;

        void Awake()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onUpdate += Redraw;
        }

        void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI questItemUi = Instantiate<QuestItemUI>(questPrefab, transform);
                questItemUi.Setup(status);
            }
        }
    }
}
