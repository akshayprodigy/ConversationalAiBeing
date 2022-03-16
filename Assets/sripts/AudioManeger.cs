using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    private void Awake()
    {
        
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        CallRestHandler.OnAudioLoaded += PlayAudio;
        SampleSpeechToText.OnAudioPaused += PauseAudio;
    }

    private void OnDisable()
    {
        CallRestHandler.OnAudioLoaded -= PlayAudio;
        SampleSpeechToText.OnAudioPaused -= PauseAudio;
    }
    public void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayAudio()
    {
        //Debug.Log("zee play audio");
        if(source == null)
            source = GetComponent<AudioSource>();
        source.clip = CallRestHandler.audioTrack;
        Invoke("Play", 0.5f);
        
    }

    public void Play()
    {
        source.Play();
    }

    public void PauseAudio()
    {
        source.Pause();
    }

    public void DeleteClip()
    {

    }

}
