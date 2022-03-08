using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] GameObject menu;
        [SerializeField] GameObject settings;

        SavingManager savingManager;

        private void Start() 
        {
            OpenSettingsMenu(false);
            savingManager = FindObjectOfType<SavingManager>();
            if(savingManager.LoadingScreen.activeInHierarchy)
            {
                StartCoroutine(DiseableLoadingScreen());
            } 
        }

        public void StartNewGame()
        {
            savingManager.Delete();
            savingManager.StartGame();
        }

        public void LoadGame()
        {
            savingManager.StartGame();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void OpenSettingsMenu(bool open)
        {
            menu.gameObject.SetActive(!open);
            settings.gameObject.SetActive(open);
        } 

        private IEnumerator DiseableLoadingScreen()
        {
            yield return new WaitForSeconds(2f);
            savingManager.LoadingScreen.SetActive(false);
        }
             
    }
}