using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundController : MonoBehaviour
{
    /*[SerializeField] GameObject audioSources;

    [SerializeField] AudioScriptableObject[] sounds;

    private AudioSource[] allAudio;

    [SerializeField]bool[] doNotPlay;

    private void Awake()
    {
        GenerateAudioComponentList();

        //generate the true false array
        doNotPlay = new bool[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            doNotPlay[i] = false;
        }

        SoundManager.stopAllAudio += StopAudio;

        allAudio = GetComponentsInChildren<AudioSource>();
    }

    void GenerateAudioComponentList()
    {
        //create all the audio sources
        foreach (AudioScriptableObject s in sounds)
        {
            s.source = audioSources.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.group;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
            s.source.spatialBlend = s.spatialBlend;
        }
    }

    //stop all audio event
    public void StopAudio()
    {
        foreach (AudioSource audioSource in allAudio)
        {
            audioSource.Stop();
        }
    }

    //plays Audio
    public void PlaySound(int index)
    {
        if (allAudio[index] == null)
            return;
        allAudio[index].Play();
    }

    //Stops  Audio
    public void StopSound(int index)
    {
        if (allAudio[index] == null)
            return;
        allAudio[index].Stop();
    }

    //play a one shot that does not need to be interrupted, can have mutiple instances
    public void PlayOneShotSound(int index)
    {
        if (doNotPlay[index] == false)
        {
            if (allAudio[index] == null)
                return;
            allAudio[index].PlayOneShot(allAudio[index].clip, allAudio[index].volume);
        }
        else
        {
            return;
        }
        
    }

    public void CheckIfPlaying(int index)
    {
        if (allAudio[index].isPlaying == true)
        {
            doNotPlay[index] = true;
        }
        else if (allAudio[index].isPlaying == false)
        {
            doNotPlay[index] = false;
        }
    }*/
}
