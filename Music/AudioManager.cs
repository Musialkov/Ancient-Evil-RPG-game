using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Music
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup mixer;
        [SerializeField] float timeToFade;
        public AudioClip DefaultTrack {get; set;}
        [Range(0, 1)][SerializeField] float defaultVolume;
        private AudioSource track1, track2;
        private bool isPlayingTrack1;
        
        private void Awake() 
        {
            track1 = gameObject.AddComponent<AudioSource>();
            track2 = gameObject.AddComponent<AudioSource>();
            
            track1.outputAudioMixerGroup = mixer;
            track2.outputAudioMixerGroup = mixer;

            track1.loop = true;
            track2.loop = true;
            isPlayingTrack1 = true;        
        }

        private void Start() 
        {
           // StartCoroutine(LateStart(1));
        }

        IEnumerator LateStart(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            SwapTrack(DefaultTrack);
        }

        public void SwapTrack(AudioClip newClip)
        {           
            StopAllCoroutines();
            StartCoroutine(FadeTrack(newClip));

            isPlayingTrack1 = !isPlayingTrack1;
        }

        public void ReturnTuDefaultTrack()
        {
            SwapTrack(DefaultTrack);
        }

        private IEnumerator FadeTrack(AudioClip newClip)
        {
            float timeElapsed = 0;
            if (isPlayingTrack1)
            {
                track2.clip = newClip;
                track2.Play();

                while(timeElapsed < timeToFade)
                {
                    track2.volume = Mathf.Lerp(0, defaultVolume, timeElapsed / timeToFade);
                    track1.volume = Mathf.Lerp(defaultVolume, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                track1.Stop();
            }
            else
            {
                track1.clip = newClip;
                track1.Play();
                while (timeElapsed < timeToFade)
                {
                    track1.volume = Mathf.Lerp(0, defaultVolume, timeElapsed / timeToFade);
                    track2.volume = Mathf.Lerp(defaultVolume, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                track2.Stop();
            }
        }
    }
}