using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;

        static bool isOnScene = false;

        private void Awake() {
            
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