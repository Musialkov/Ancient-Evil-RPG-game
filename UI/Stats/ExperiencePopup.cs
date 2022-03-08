using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.Stats
{
    public class ExperiencePopup : MonoBehaviour
    {
        [SerializeField] CanvasGroup experiencePopupCanvas;
        [SerializeField] TMP_Text text;

        public IEnumerator ShowPopup(float gainedExperience)
        {
            text.text = "Experience + " + gainedExperience;

            experiencePopupCanvas.alpha = 1;
            yield return new WaitForSeconds(3f);
            LeanTween.alphaCanvas(experiencePopupCanvas, 0, 0.5f);
        }
    }
}
