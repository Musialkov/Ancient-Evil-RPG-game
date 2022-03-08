using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music
{
    public class AudioSwapper : MonoBehaviour
    {
        [SerializeField] AudioClip track;
        AudioManager audioManager;

        private void Awake() 
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                audioManager.SwapTrack(track);
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
                audioManager.ReturnTuDefaultTrack();
            }
        }
    }
}
