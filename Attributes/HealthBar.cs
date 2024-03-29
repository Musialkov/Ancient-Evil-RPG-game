﻿using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Canvas healthBarCanvas = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Health health = null;
        
        private void Start() 
        {
            healthBarCanvas.enabled = false;
        }
        private void Update() 
        {
            if(health.GetFraction() < 1 && health.GetFraction() > 0)
            {
                healthBarCanvas.enabled = true;
                SetHealthBar();
            }
            else
            {
                healthBarCanvas.enabled = false;
            }
               
        }
        private void SetHealthBar()
        {
            Vector3 newScale = new Vector3(health.GetFraction(), 1, 1);
            foreground.localScale = newScale;
        }
    }
}
