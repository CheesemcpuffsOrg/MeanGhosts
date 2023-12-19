using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Joshua 

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        private class AudioReference
        {
            public string name;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
        }

        public static AudioManager AudioManagerInstance;

        [SerializeField]private SoundDisk[] soundDisks;
        List<AudioReference> audioReference = new List<AudioReference>();
        Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] GameObject audioObjPrefab;

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        private void Awake()
        {
            AudioManagerInstance = this;

            foreach(SoundDisk soundDisk in soundDisks) 
            { 
                if(soundDisk == null)
                {
                    Debug.LogError("One or more sound disk is missing from the array.");
                    break;
                }
            }

        }

        /// <summary>
        /// Stops all audio
        /// </summary>
        public void StopAllAudio()
        {
            foreach (AudioReference s in audioReference)
            {
                s.audioSource.transform.SetParent(audioPoolContainer);
                audioPool.Enqueue(s.audioSource);
                s.audioSource.GetComponent<AudioSource>().Stop();
                if (s.clipLength != null)
                {
                    StopCoroutine(s.clipLength);
                }
                audioReference.Remove(s);
            }
        }

        /// <summary>
        /// Play a sound.
        /// </summary>
        public void PlaySound(AudioScriptableObject sound, SoundDisk soundDisk, GameObject gameObject)
        { 
            string soundDiskName = soundDisk.name;

            string soundName = sound.name;

            SoundDisk disk = Array.Find(soundDisks, obj => obj.name == soundDiskName);

            if (disk == null)
            {
                Debug.LogError("Sound Disk: " + soundName + " is not set up. Please create a sound disk.");
                return;
            }

            AudioScriptableObject s = disk.FindSound(soundName);

            if (s == null)
            {
                Debug.LogError("Sound: " + soundName + " does not exist.");
            }

            AudioSource audioSource = null;
            GameObject obj = null;
            Coroutine clipLength = null;

            if (audioPool.Count <= 0)
            {
                obj = Instantiate(audioObjPrefab);
            }
            else
            {
                obj = audioPool.Dequeue();
            }

            audioSource = obj.GetComponent<AudioSource>();

            audioSource.clip = s.clip;
            audioSource.outputAudioMixerGroup = s.group;
            audioSource.volume = s.volume;
            audioSource.pitch = s.pitch;
            audioSource.loop = s.loop;
            audioSource.panStereo = s.pan;
            audioSource.spatialBlend = s.spatialBlend;
            audioSource.rolloffMode = s.rolloffMode;
            audioSource.minDistance = s.minDistance;
            audioSource.maxDistance = s.maxDistance;
            audioSource.dopplerLevel = s.dopplerLevel;

            if (audioSource.spatialBlend == 0)
            {
                obj.transform.SetParent(activeSounds);
                obj.transform.position = activeSounds.position;
            }
            else
            {
                obj.transform.SetParent(gameObject.transform);
                obj.transform.position = gameObject.transform.position;
            }

            AudioReference createdObjReference = new AudioReference();

            createdObjReference.name = soundName;
            createdObjReference.requestingObj = gameObject;
            createdObjReference.audioSource = obj;

            if (!audioSource.loop)
            {
                clipLength = StartCoroutine(Countdown(audioSource.clip.length, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            audioReference.Add(createdObjReference);

            audioSource.Play();
        }

        IEnumerator Countdown(float seconds, AudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i <= audioReference.Count; i++)
            {
                if (audioReference[i].name == reference.name && audioReference[i].requestingObj == reference.requestingObj)
                {
                    audioReference[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(audioReference[i].audioSource);
                    audioReference.RemoveAt(i);
                    yield break;
                }
            }
        }

        /// <summary>
        /// Stops an active sound.
        /// </summary>
        public void StopSound(string soundName, GameObject gameObject)
        {
            int i;

            for (i = 0; i < audioReference.Count; i++)
            {
                if (audioReference[i].name == soundName && audioReference[i].requestingObj == gameObject)
                {
                    audioReference[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(audioReference[i].audioSource);
                    audioReference[i].audioSource.GetComponent<AudioSource>().Stop();
                    if (audioReference[i].clipLength != null)
                    {
                        StopCoroutine(audioReference[i].clipLength);
                    }
                    audioReference.RemoveAt(i);
                    return;
                }
            }

            if (audioReference[i] != null)
            {
                Debug.LogWarning("Sound: " + soundName + " is not active.");
            }
        }

        /// <summary>
        /// Allows you to delay the activation of a sound.
        /// </summary>
        public void DelayedPlaySound(float delay, AudioScriptableObject sound, SoundDisk soundDisk, GameObject gameObject)
        {
            StartCoroutine(SoundDelay(delay, sound, soundDisk, gameObject));
        }

        public IEnumerator SoundDelay(float delay, AudioScriptableObject sound, SoundDisk soundDisk, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(sound, soundDisk, gameObject);
        }

        //need to create method for fade in and fade out, do not put this on the SO, there is more control with a method.
        //Handle mutiple audio source stacking.
        //Make one shots if neccesary, don't see a point though.
    }
}
