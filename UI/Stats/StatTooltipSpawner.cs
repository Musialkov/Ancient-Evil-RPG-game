using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using RPG.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Stats
{
    public class StatTooltipSpawner : TooltipSpawner
    {
        [SerializeField] Stat stat;

        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            Debug.Log("KK");
            tooltip.GetComponent<StatTooltipUI>().Setup(stat);
        }
       
    }
}
