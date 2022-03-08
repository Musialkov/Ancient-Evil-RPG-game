using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Control
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
        public static bool GameIsPaused {get; set;}
        GameObject uiCanvas;
        PlayerController player;
        SavingManager savingManager;

        void Start()
        {
            pauseMenu.SetActive(false);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            uiCanvas = GameObject.FindGameObjectWithTag("UICanvas");
            savingManager = FindObjectOfType<SavingManager>();
        }


        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (player.enabled == true && !GameIsPaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }           
        }

        public void Pause()
        {
            GameIsPaused = true;            
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            GameIsPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Load()
        {
            StartCoroutine(savingManager.Load()); 
            Resume();
        }

        public void Save()
        {            
            savingManager.Save();
        }

        public void LoadMenu()
        {
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;

            int buildIndex = (int)Scenes.Tavern;
            foreach (var scene in SceneManager.GetAllScenes())
            {
                if (scene.buildIndex != (int)Scenes.PersistantScene)
                {
                    buildIndex = scene.buildIndex;
                }
            }

            savingManager.LoadingScreen.SetActive(true);
            scenesLoading.Add(SceneManager.UnloadSceneAsync(buildIndex));
            scenesLoading.Add(SceneManager.LoadSceneAsync((int)Scenes.Menu, LoadSceneMode.Additive));

            //StartCoroutine(savingManager.GenerateTips());
            StartCoroutine(LoadingMenu());                      
        }

        private IEnumerator LoadingMenu()
        {
            for (int i = 0; i < scenesLoading.Count; i++)
            {
                while (!scenesLoading[i].isDone)
                {      
                    yield return null;
                }
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)Scenes.Menu));                       
            yield return new WaitForSeconds(3f);
            savingManager.LoadingScreen.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
