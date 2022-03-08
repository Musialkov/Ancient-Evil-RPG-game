using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Music
{
    public class AudioDefaultChanger : MonoBehaviour
    {
        [SerializeField] AudioClip defaultMusic;
        void Start()
        {
            var audio = GameObject.FindObjectOfType<AudioManager>();
            if(audio && defaultMusic != null)
            {
                audio.DefaultTrack = defaultMusic;
                audio.ReturnTuDefaultTrack();
            }
        }
    }
}
