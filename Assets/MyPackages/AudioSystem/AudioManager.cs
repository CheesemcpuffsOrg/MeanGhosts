using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

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
            public ScriptableObject objReference;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
            public UnityEvent endOfClip;
        }

        public static AudioManager AudioManagerInstance;

        List<AudioReference> audioReference = new List<AudioReference>();
        Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] GameObject audioObjPrefab;

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        //bool reduceOtherAudios = false;

        private void Awake()
        {
            AudioManagerInstance = this;
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

            //take obj from audio pool, if there are none create a new object
            if (audioPool.Count > 0)
            {
                obj = audioPool.Dequeue();
                
            }
            else
            {
                obj = Instantiate(audioObjPrefab);
            }

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

/*            //this causes specific sounds to play over other sounds
            if (sound.reduceAllOtherAudioButThis)
            {
                reduceOtherAudios = true;
                foreach (AudioReference s in audioReference)
                {
                    var currentVolume = s.audioSource.GetComponent<AudioSource>().volume;

                    s.audioSource.GetComponent<AudioSource>().volume =  currentVolume - (currentVolume * 60/100)
                }
            }*/

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

            createdObjReference.objReference = sound;
            createdObjReference.requestingObj = gameObject;
            createdObjReference.audioSource = obj;

            if (!audioSource.loop)
            {
                clipLength = StartCoroutine(Countdown(audioSource.clip.length, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            createdObjReference.endOfClip = new UnityEvent();

            audioReference.Add(createdObjReference);

            audioSource.Play();
        }

        IEnumerator Countdown(float seconds, AudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i <= audioReference.Count; i++)
            {
                if (audioReference[i].objReference == reference.objReference && audioReference[i].requestingObj == reference.requestingObj)
                {
                    audioReference[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioReference[i].endOfClip.Invoke();
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
                if (audioReference[i].objReference == sound && audioReference[i].requestingObj == gameObject)
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
        public void PlaySoundFromlist(List<AudioScriptableObject> list, GameObject gameObject)
        {
            var randomList = RandomUtility.RandomListSort(list);

            var sound = randomList[0];

            PlaySound(sound, gameObject);
        }


        /// <summary>
        /// Checks to see if the requested sound exists and is playing
        /// </summary>
        public void PlaySoundIfNotAlreadyPlaying(AudioScriptableObject sound, GameObject gameObject)
        {
            int i;

            for (i = 0; i < audioReference.Count; i++)
            {
                if (audioReference[i].objReference == sound && audioReference[i].requestingObj == gameObject)
                {
                    return;
                }
            }
            PlaySound(sound, gameObject);
        }

        /// <summary>
        /// Fires an event when an audio source stops playing
        /// </summary>
        public void FireEventWhenSoundFinished(AudioScriptableObject sound, GameObject gameObject, UnityAction reference)
        {
            int i;

            for (i = 0; i < audioReference.Count; i++)
            {
                if (audioReference[i].objReference == sound && audioReference[i].requestingObj == gameObject)
                {
                    audioReference[i].endOfClip.AddListener(reference);
                }
            }
        }
    }
}
