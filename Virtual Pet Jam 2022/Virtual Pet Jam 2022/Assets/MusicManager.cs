using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> backgroundMusic;
    [SerializeField] Image pauseButton;

    bool paused = false;
    AudioSource audioSource;
    float timeBeforeNextClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused) {
            timeBeforeNextClip -= Time.deltaTime;
            if (timeBeforeNextClip <= 0f) {
                TransitionAudioClip();
            }
        }
    }


    void TransitionAudioClip() {
        int soundID = Random.Range(0, backgroundMusic.Count - 1);
        audioSource.clip = backgroundMusic[soundID];
        timeBeforeNextClip = backgroundMusic[soundID].length;
        audioSource.Play();
    }


    public void ToggleMusic() {
        if(paused) {
            //Unpause
            audioSource.UnPause();
            paused = false;

            //Change color
            Color color = pauseButton.color;
            color.a = 1f;
            pauseButton.color = color;
        } else {
            //Pause
            audioSource.Pause();
            paused = true;

            //Change color
            Color color = pauseButton.color;
            color.a = .5f;
            pauseButton.color = color;
        }
    }
}
