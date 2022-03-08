using System.Collections;
using System.Collections.Generic;
using Musialkov.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.StoryLine
{
    public class CinematicTrigger : MonoBehaviour
    {

        bool alreadyPlayed = false;

        private void OnTriggerEnter(Collider other) {
            Debug.Log(alreadyPlayed);
            Debug.Log(other.gameObject.tag);
            if(!alreadyPlayed && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                alreadyPlayed = true;
                Debug.Log("dupsko");
            }
        }

        //public object CaptureState()
        //{
        //    return alreadyPlayed;
        //}
//
        //public void RestoreState(object state)
        //{
        //    alreadyPlayed = (bool) state;
        //}
    }
}
