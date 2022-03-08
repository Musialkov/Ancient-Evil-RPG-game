using System.Collections;
using UnityEngine;
using Musialkov.Saving;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

namespace RPG.SceneManagement
{
    public class SavingManager : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.3f;
        [SerializeField] public GameObject LoadingScreen;
        [SerializeField] TextMeshProUGUI tipText;
        [SerializeField] CanvasGroup aplhaCanvas;
        [SerializeField] string[] tips;

        int tipCount;
        static string defaultSaveFile = "save";
        public static SavingManager instance;
        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        private void Awake() 
        {
            instance = this;
            LoadingScreen.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync((int) Scenes.Menu, LoadSceneMode.Additive);
        }
     
        void Update()
        {            
            if(Input.GetKeyDown(KeyCode.F12))
            {
                Delete();
            }
        }

        public void StartGame()
        {
            LoadingScreen.gameObject.SetActive(true);
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)Scenes.Menu));

            Dictionary<string, object> state = GetComponent<SavingSystem>().LoadFile(defaultSaveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex + 2;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            scenesLoading.Add(SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive));
            StartCoroutine(GetSceneLoadProgress(state, buildIndex));
            StartCoroutine(GenerateTips());
        }

        private IEnumerator GetSceneLoadProgress(Dictionary<string, object> state, int buildIndex)
        {
            for(int i = 0; i < scenesLoading.Count; i++)
            {
                while(!scenesLoading[i].isDone)
                {
                    yield return null;
                }
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));          
            GetComponent<SavingSystem>().RestoreState(state);
            yield return new WaitForSeconds(3);
            LoadingScreen.gameObject.SetActive(false);
        }

        public IEnumerator GenerateTips()
        {
            tipCount = Random.Range(0, tips.Length);
            tipText.text = tips[tipCount];

            while(LoadingScreen.activeInHierarchy)
            {
                yield return new WaitForSeconds(3f);

                LeanTween.alphaCanvas(aplhaCanvas, 0, 0.5f);

                yield return new WaitForSeconds(0.5f);

                tipCount++;
                if(tipCount >= tips.Length)
                {
                    tipCount = 0;
                }

                tipText.text = tips[tipCount];

                LeanTween.alphaCanvas(aplhaCanvas, 1, 0.5f);
            }
        }

        public IEnumerator Load()
        {
            LoadingScreen.gameObject.SetActive(true);
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            yield return new WaitForSeconds(3);
            LoadingScreen.gameObject.SetActive(false);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}