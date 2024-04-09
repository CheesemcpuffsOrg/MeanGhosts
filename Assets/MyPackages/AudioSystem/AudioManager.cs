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

    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        private class AudioReference
        {
            public AudioScriptableObject objReference;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
            public UnityEvent endOfClip;
        }

        public static AudioManager AudioManagerInstance;

        List<AudioReference> audioReferences = new List<AudioReference>();
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
            foreach (AudioReference s in audioReferences)
            {
                s.audioSource.transform.SetParent(audioPoolContainer);
                audioPool.Enqueue(s.audioSource);
                s.audioSource.GetComponent<AudioSource>().Stop();
                if (s.clipLength != null)
                {
                    StopCoroutine(s.clipLength);
                }
                audioReferences.Remove(s);
            }
        }

        /// <summary>
        /// Call this method when you pause the game.
        /// </summary>
        public void PauseAllAudio()
        {
            foreach (AudioReference s in audioReferences)
            {
                if (!s.objReference.playWhilePaused)
                {
                    s.audioSource.GetComponent<AudioSource>().Pause();
                }
            }
        }

        /// <summary>
        ///  Call this method when you unpause the game.
        /// </summary>
        public void UnPauseAllAudio()
        {
            foreach (AudioReference s in audioReferences)
            {
                if (!s.objReference.playWhilePaused)
                {
                    s.audioSource.GetComponent<AudioSource>().Play();
                }
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
                return;
            }

            AudioSource audioSource = obj.GetComponent<AudioSource>();

            audioSource.clip = (AudioClip)RandomUtility.ObjectPoolCalculator(sound.audioClips);
            audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;
            var fadeIn = sound.fadeIn;
            var fadeInDuration = sound.fadeInDuration;
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

            createdObjReference.objReference = sound;
            createdObjReference.requestingObj = gameObject;
            createdObjReference.audioSource = obj;

            if (!audioSource.loop)
            {
                clipLength = StartCoroutine(Countdown(audioSource.clip.length, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            createdObjReference.endOfClip = new UnityEvent();

            audioReferences.Add(createdObjReference);

            audioSource.Play();

            if (fadeIn)
            {
                StartCoroutine(FadeIn(audioSource, fadeInDuration, sound.volume));
            }
        }

        IEnumerator Countdown(float seconds, AudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i <= audioReferences.Count; i++)
            {
                if (audioReferences[i].objReference == reference.objReference && audioReferences[i].requestingObj == reference.requestingObj)
                {
                    audioReferences[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioReferences[i].endOfClip.Invoke();
                    audioPool.Enqueue(audioReferences[i].audioSource);
                    audioReferences.RemoveAt(i);
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

            for (i = 0; i < audioReferences.Count; i++)
            {
                if (audioReferences[i].objReference == sound && audioReferences[i].requestingObj == gameObject)
                {
                    audioReferences[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(audioReferences[i].audioSource);

                    var audioSource = audioReferences[i].audioSource.GetComponent<AudioSource>();

                    if (sound.fadeOut)
                    {
                        StartCoroutine(FadeOut(audioSource, sound.fadeOutDuration, audioSource.volume));
                    }
                    else
                    {
                        audioSource.Stop();
                        if (audioReferences[i].clipLength != null)
                        {
                            StopCoroutine(audioReferences[i].clipLength);
                        }
                        audioReferences.RemoveAt(i);
                    }
                    return;
                }
            }

            if (audioReferences[i] != null)
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

        private IEnumerator SoundDelay(float delay, AudioScriptableObject sound, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(sound, gameObject);
        }


        //might rework this to just check if sound is playing, much more useful, get it to return bool is sound is playing

        /// <summary>
        /// Checks to see if the requested sound exists and is playing
        /// </summary>
        public bool IsSoundPlaying(AudioScriptableObject sound, GameObject gameObject)
        {
            int i;

            for (i = 0; i < audioReferences.Count; i++)
            {
                if (audioReferences[i].objReference == sound && audioReferences[i].requestingObj == gameObject)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fires an event when an audio source stops playing.
        /// </summary>
        public void FireEventWhenSoundFinished(AudioScriptableObject sound, GameObject gameObject, UnityAction reference)
        {
            int i;

            for (i = 0; i < audioReferences.Count; i++)
            {
                if (audioReferences[i].objReference == sound && audioReferences[i].requestingObj == gameObject)
                {
                    audioReferences[i].endOfClip.AddListener(reference);
                }
            }
        }

        /// <summary>
        /// Fade in audio source.
        /// </summary>
        private static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// Fade out audio source.
        /// </summary>
        private static IEnumerator FadeOut(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = targetVolume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                yield return null;
            }
            audioSource.Stop();
            yield break;
        }

        #region -- DEPRECATED --
        /// <summary>
        /// Plays a random sound from a list.
        /// </summary>
        /*public void PlaySoundFromlist(List<AudioScriptableObject> list, GameObject gameObject)
        {
            var randomList = RandomUtility.RandomListSort(list);

            var sound = randomList[0];

            PlaySound(sound, gameObject);
        }*/
        #endregion
    }
}
