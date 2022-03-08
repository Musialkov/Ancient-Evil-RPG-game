using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;

        static bool isOnScene = false;

        private void Awake() {

            //SceneManager.LoadScene(3);    
            
            if(isOnScene) return;

            ActivatePersistanObject();

            isOnScene = true;
        }

        private void ActivatePersistanObject()
        {
            GameObject persistanObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistanObject);
        }
    }
}