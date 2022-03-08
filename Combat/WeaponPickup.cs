using UnityEngine;
using System.Collections;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float timeOfHiding = 5f;

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideObject(timeOfHiding));
        }

        private IEnumerator HideObject(float timeOfHiding)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(timeOfHiding);
            ShowPickup(true);
        }

        private void ShowPickup(bool showPickup)
        {
            GetComponent<Collider>().enabled = showPickup;
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(showPickup);
            }
        }

        public bool HandleRaycast(PlayerController player)
        {
           if(Input.GetMouseButtonDown(0))
           {
               Pickup(player.GetComponent<Fighter>());              
           }
           return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}