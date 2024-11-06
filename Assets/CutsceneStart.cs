using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SignalReceiver))]
public class CutsceneStart : MonoBehaviour
{
    public PlayableDirector cutscene;

    public UnityEvent OnPlay;
    public UnityEvent OnStop;

    public bool playerOnTrigger;

    public bool playerIsTrigger;
    private bool alreadyPlayed;

    void PlayCutscene()
    {
    if (alreadyPlayed)
    {
        return;
    }
  
        alreadyPlayed = true;
        OnPlay.Invoke();
        cutscene.Play();
        Invoke("FinishCutscene", (float)cutscene.duration);
    }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsTrigger = true;
                if (playerOnTrigger)
                {

                }
            }
    }
    void FinishCutscene()
    {
        OnStop.Invoke ();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsTrigger= false;
        }
    }

}

