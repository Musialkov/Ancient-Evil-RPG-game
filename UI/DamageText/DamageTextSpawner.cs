using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageText = null;

        public void Spawn(float damageAmount)
        {
            DamageText damageTextinstance = Instantiate<DamageText>(damageText, transform);
            damageTextinstance.SetValue(damageAmount);
        }
    }

}