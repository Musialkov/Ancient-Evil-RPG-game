using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    enum PortalDestination 
    {
        MonasteryPass, TemplePass
    }

    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoadIndex = 0;
        [SerializeField] int currentSceneIndex = 0;

        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalDestination destination;
        [SerializeField] float waitTimeBetweenScenes;
        [SerializeField] float fadeOutTime;
        [SerializeField] float fadeInTime;

        Fader fader;
        SavingManager savingManager;
        BoxCollider boxCollider;

        private void Awake() 
        {
            savingManager = FindObjectOfType<SavingManager>();
            fader = FindObjectOfType<Fader>();
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }            
        } 

        public void LaunchThePortal()
        {
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);
            boxCollider.isTrigger = false;   
                     
            savingManager.LoadingScreen.gameObject.SetActive(true);
            StartCoroutine(savingManager.GenerateTips());

            SwitchPlayerControll(false);
            yield return fader.FadeOut(fadeOutTime);
          
            savingManager.Save();

            yield return SceneManager.UnloadSceneAsync(currentSceneIndex);
            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoadIndex));
            SwitchPlayerControll(false);

            yield return savingManager.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerTransform(otherPortal);

            savingManager.Save();

            yield return new WaitForSeconds(waitTimeBetweenScenes);
            yield return fader.FadeIn(fadeInTime);

            savingManager.LoadingScreen.gameObject.SetActive(false);
            
            SwitchPlayerControll(true);
            Destroy(gameObject);
        }

        private static void SwitchPlayerControll(bool turnOn)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = turnOn;
        }

        private Portal GetOtherPortal() 
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal != this && portal.destination == destination)
                {
                    return portal;
                }                
            }
            return null;
        }

        private void UpdatePlayerTransform(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
