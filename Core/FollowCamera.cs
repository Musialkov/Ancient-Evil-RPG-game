using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target;

        void LateUpdate()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            transform.position = target.transform.position;
        }
    }

}