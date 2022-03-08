using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffects : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if(targetToDestroy == null)
                {
                    targetToDestroy = gameObject;
                }
                Destroy(targetToDestroy);
            }
        }
    }
}
