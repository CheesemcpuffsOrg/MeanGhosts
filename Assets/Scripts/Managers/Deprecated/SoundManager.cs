using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SoundManagerInstance;

    //for storing currently used sounds
    [SerializeField]private AudioScriptableObject[] sounds;

    private List<AudioScriptableObject> arrayStorage = new List<AudioScriptableObject>();
    private List<AudioSource> audioStorage = new List<AudioSource>();

    //private AudioSource[] allAudio;

    public static event Action stopAllAudio;

    private void Awake()
    {
        SoundManagerInstance = this;

        /*GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);*/

        PopulateAudioManager();

    }

    private void Start()
    {
        sounds = arrayStorage.ToArray();//store all the list values to an array
    }

    void PopulateAudioManager()
    {
        for (int i = 0; i < 32; i++)
        {
            this.gameObject.AddComponent<AudioSource>();
        }

        foreach (AudioSource s in GetComponents<AudioSource>())
        {
            s.GetComponents<AudioSource>();
            audioStorage.Add(s);
        }
    }

    //The sound disks call this to add their sounds to the audio list
    public void GenerateAudioComponentList(AudioScriptableObject[] audioList)
    {
        arrayStorage.AddRange(audioList);//add all the array values to a list
    }

    //stops all audio when called
    public void StopAllAudio()
    {
        stopAllAudio();
    }

    public int PlaySound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

        int i;

        for (i = 0; i <= audioStorage.Count; i++)
        {
            if (audioStorage[i].isPlaying == false)
            {
                AudioSource audioSource = audioStorage[i];

                audioSource.clip = s.clip;

                audioSource.outputAudioMixerGroup = s.group;
                audioSource.volume = s.volume;
                audioSource.pitch = s.pitch;
                audioSource.loop = s.loop;
                audioSource.panStereo = s.pan;
                audioSource.Play();
                return i;
            }
        }
        return i;
        
    }

    public void StopSound(int arrayNumber)
    {
        AudioSource audioSource = audioStorage[arrayNumber];

        audioSource.Stop();
    }

    public void PlayOneShotSound(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

        for (int i = 0; i < audioStorage.Count; i++)
        {
            if (audioStorage[i].isPlaying == false)
            {
                AudioSource audioSource = audioStorage[i];

                audioSource.clip = s.clip;

                audioSource.outputAudioMixerGroup = s.group;
                audioSource.volume = s.volume;
                audioSource.pitch = s.pitch;
                audioSource.loop = s.loop;
                audioSource.panStereo = s.pan;
                audioSource.PlayOneShot(s.clip, s.volume);

                return;
            }
        }
    }

    public void PlaySoundAtLoation(string name, GameObject obj)
    {

    }

    //plays single instance of menu Audio
    public void PlaySoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
        s.source.Play();
    }

    //Stops single instance of menu Audio
    public void StopSoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
        s.source.Stop();
    }

    //play a one shot that does not need to be interrupted in the menu, can have mutiple instances
    public void PlayOneShotSoundMenu(string name)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.ignoreListenerPause = true;
        s.source.PlayOneShot(s.clip, s.volume);
    }

    //rework this to switch between the two
    public void PauseAllAudio()
    {
        AudioListener.pause = true;
    }
}
