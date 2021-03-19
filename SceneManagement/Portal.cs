using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] Transform spawnPoint;
        [SerializeField] PortalDestination destination;
        [SerializeField] float waitTimeBetweenScenes;
        [SerializeField] float fadeOutTime;
        [SerializeField] float fadeInTime;


        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }            
        } 

        private IEnumerator Transition()
        {    
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavingManager savingManager = FindObjectOfType<SavingManager>();
            savingManager.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);    

            savingManager.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayerTransform(otherPortal);

            savingManager.Save();  

            yield return new WaitForSeconds(waitTimeBetweenScenes);
            yield return fader.FadeIn(fadeInTime);
            
            Destroy(gameObject);
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
