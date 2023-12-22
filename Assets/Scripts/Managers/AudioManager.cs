using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Joshua 

namespace AudioSystem
{

    //add fade in fade out to scriptable object.
    //Handle mutiple audio source stacking.
    //Make one shots if neccesary, don't see a point though.
    //have method that takes in group of sounds, picks a random sound and passes it through play

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

       // [SerializeField]private SoundDisk[] soundDisks;
        List<AudioReference> audioReference = new List<AudioReference>();
        Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] GameObject audioObjPrefab;

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        private void Awake()
        {
            AudioManagerInstance = this;

            /*foreach(SoundDisk soundDisk in soundDisks) 
            { 
                if(soundDisk == null)
                {
                    Debug.LogError("One or more sound disk is missing from the array.");
                    break;
                }
            }*/

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
        public void PlaySound(AudioScriptableObject sound, GameObject gameObject)
        {
            Coroutine clipLength = null;

            GameObject obj;

            if (audioPool.Count <= 0)
            {
                obj = Instantiate(audioObjPrefab);
            }
            else
            {
                obj = audioPool.Dequeue();
            }

            /*string soundDiskName = soundDisk.name;

            string soundName = sound.name;

            SoundDisk disk = Array.Find(soundDisks, obj => obj.name == soundDiskName);

            if (disk == null)
            {
                Debug.LogError("Sound Disk: " + soundName + " is not set up. Please create a sound disk.");
                return;
            }

            AudioScriptableObject s = disk.FindSound(soundName);*/

            if (sound == null)
            {
                Debug.LogError($"You are missing a sound SO from the gameobject {gameObject.name}");
            }

            AudioSource audioSource = obj.GetComponent<AudioSource>();

            audioSource.clip = sound.clip;
            audioSource.outputAudioMixerGroup = sound.group;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;
            audioSource.panStereo = sound.pan;
            audioSource.spatialBlend = sound.spatialBlend;
            audioSource.rolloffMode = sound.rolloffMode;
            audioSource.minDistance = sound.minDistance;
            audioSource.maxDistance = sound.maxDistance;
            audioSource.dopplerLevel = sound.dopplerLevel;

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

            createdObjReference.name = sound.name;
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
        public void StopSound(AudioScriptableObject sound, GameObject gameObject)
        {
            int i;

            for (i = 0; i < audioReference.Count; i++)
            {
                if (audioReference[i].name == sound.name && audioReference[i].requestingObj == gameObject)
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
                Debug.LogWarning("Sound: " + sound + " is not active.");
            }
        }

        /// <summary>
        /// Allows you to delay the activation of a sound.
        /// </summary>
        public void DelayedPlaySound(float delay, AudioScriptableObject sound, GameObject gameObject)
        {
            StartCoroutine(SoundDelay(delay, sound, gameObject));
        }

        public IEnumerator SoundDelay(float delay, AudioScriptableObject sound, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(sound, gameObject);
        }

        /// <summary>
        /// Plays a random sound from a list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="gameObject"></param>
        public void PlaySoundFromlist(List<AudioScriptableObject> list, GameObject gameObject)
        {
            var randomList = RandomUtility.RandomListSort(list);

            var sound = randomList[0];

            PlaySound(sound, gameObject);
        }

        
    }
}
