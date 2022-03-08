using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveCoroutine = null;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
           if(currentActiveCoroutine != null)
           {
               StopCoroutine(currentActiveCoroutine);
           }
           currentActiveCoroutine = StartCoroutine(FadeOutRoutine(time));
           yield return currentActiveCoroutine;
        }

        private IEnumerator FadeOutRoutine(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            if (currentActiveCoroutine != null)
            {
                StopCoroutine(currentActiveCoroutine);
            }
            currentActiveCoroutine = StartCoroutine(FadeInRoutine(time));
            yield return currentActiveCoroutine;
        }
        private IEnumerator FadeInRoutine(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}