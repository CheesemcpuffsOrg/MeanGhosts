using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

//Joshua 

namespace AudioSystem
{

    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        private class AudioReference
        {
            public AudioScriptableObject objReference;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
            public UnityEvent endOfClip;
            public float volume;
        }

        public static AudioManager AudioManagerInstance;

        List<AudioReference> audioReferences = new List<AudioReference>();
        Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] GameObject audioObjPrefab;

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        [SerializeField] int StackingAudioLimiter = 5;

        int activeAudioPriority = 0;
        float reductionAmount = 0.25f;

        private void Awake()
        {
            AudioManagerInstance = this;
        }

        #region -- PUBLIC METHODS --

        /// <summary>
        /// Play a sound.
        /// </summary>
        public void PlaySound(AudioScriptableObject sound, GameObject gameObject, UnityAction fireEventWhenSoundFinished = null)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound SO from the gameobject {gameObject.name}");
                return;
            }

            //priority system
            if(audioReferences.Count >= 32)
            {

            }

            GameObject obj;

            //take obj from audio pool, if there are none create a new object.
            if (audioPool.Count > 0)
            {
                obj = audioPool.Dequeue();
            }
            else
            {
                obj = Instantiate(audioObjPrefab);
            }

            var audioClip = (AudioClip)RandomUtility.ObjectPoolCalculator(sound.audioClips);

            AudioFloodPrevention(audioClip);

            var audioSource = obj.GetComponent<AudioSource>();

            PopulateTheAudioSource(sound, gameObject, obj, audioClip, audioSource);

            CreateAudioReference(sound, gameObject, obj, audioSource);

            audioSource.Play();

            if(fireEventWhenSoundFinished != null)
            {
                FireEventWhenSoundFinished(sound, gameObject, fireEventWhenSoundFinished);
            }
            
            var fadeIn = sound.fadeIn;
            var fadeInDuration = sound.fadeInDuration;

            if (fadeIn)
            {
                StartCoroutine(FadeIn(audioSource, fadeInDuration, audioSource.volume));
            }
        }

        /// <summary>
        /// Stops an active sound.
        /// </summary>
        public void StopSound(AudioScriptableObject sound, GameObject gameObject)
        {

            foreach (var s in audioReferences)
            {
                if (s.objReference == sound && s.requestingObj == gameObject)
                {
                    s.audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(s.audioSource);

                    var audioSource = s.audioSource.GetComponent<AudioSource>();

                    if (sound.fadeOut)
                    {
                        StartCoroutine(FadeOut(audioSource, sound.fadeOutDuration, audioSource.volume));
                    }
                    else
                    {
                        audioSource.Stop();
                    }

                    if (s.clipLength != null)
                    {
                        StopCoroutine(s.clipLength);
                    }

                    if (s.endOfClip != null)
                    {
                        s.endOfClip.Invoke();
                    }

                    audioReferences.Remove(s);

                    return;
                }
            }

            Debug.LogError("Sound: " + sound + " is not active.");
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

                if (s.endOfClip != null)
                {
                    s.endOfClip.Invoke();
                }

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
        /// Allows you to delay the activation of a sound.
        /// </summary>
        public void DelayedPlaySound(float delay, AudioScriptableObject sound, GameObject gameObject)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound SO from the gameobject {gameObject.name}");
                return;
            }

            StartCoroutine(SoundDelay(delay, sound, gameObject));
        }

        /// <summary>
        /// Checks to see if the requested sound exists and is playing.
        /// </summary>
        public bool IsSoundPlaying(AudioScriptableObject sound, GameObject gameObject)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound SO from the gameobject {gameObject.name}");
                return false;
            }

            foreach (var s in audioReferences)
            {
                if (s.objReference == sound && s.requestingObj == gameObject)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fires an event when an audio source stops playing.
        /// </summary>
        private void FireEventWhenSoundFinished(AudioScriptableObject sound, GameObject gameObject, UnityAction reference)
        {
            //reverse the list so it grabs the most recent sound that was added for tracking
            var reversableList = audioReferences;
            reversableList.Reverse();

            foreach (var s in reversableList)
            {
                if (s.objReference == sound && s.requestingObj == gameObject)
                {
                    s.endOfClip.AddListener(reference);
                    return;
                }
            } 
        } 

        /// <summary>
        /// The following method allows for dynamic control over the lowering and highering of all audios based on the priority of the SO parameter .
        /// </summary>
        public void DynamicVolumePrioritySystem(AudioScriptableObject sound, bool systemIsActive)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound SO from the gameobject {gameObject.name}");
                return;
            }

            if (systemIsActive && sound.audioPriority >= activeAudioPriority)
            {
                activeAudioPriority = sound.audioPriority;

                foreach (var s in audioReferences)
                {
                    if(s.objReference.audioPriority < activeAudioPriority)
                    {
                        var volume = s.audioSource.GetComponent<AudioSource>().volume;
                        s.audioSource.GetComponent<AudioSource>().volume = volume * reductionAmount;
                    }
                }
            }
            else if (!systemIsActive)
            {
                activeAudioPriority = 0;

                foreach (var s in audioReferences)
                {
                    if (s.objReference.audioPriority < activeAudioPriority)
                    {
                        s.audioSource.GetComponent<AudioSource>().volume = s.volume;
                    }
                }
            }
        }

        #endregion

        #region -- PRIVATE METHODS --

        /// <summary>
        /// This will turn any non-looping audio into a oneshot
        /// </summary>
        IEnumerator Countdown(float seconds, AudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            foreach(var s in audioReferences)
            {
                if (s.objReference == reference.objReference && s.requestingObj == reference.requestingObj)
                {
                    s.audioSource.transform.SetParent(audioPoolContainer);

                    if(s.endOfClip != null)
                    {
                        s.endOfClip.Invoke();
                    }

                    audioPool.Enqueue(s.audioSource);
                    audioReferences.Remove(s);
                    yield break;
                }
            }
        }

        private IEnumerator SoundDelay(float delay, AudioScriptableObject sound, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(sound, gameObject);
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

        /// <summary>
        /// Prevent audio flooding by deleting the oldest audio source when the limit is exceeded.
        /// </summary>
        private void AudioFloodPrevention(AudioClip audioClip)
        {
            var stack = 0;

            foreach (AudioReference s in audioReferences)
            {
                if (s.audioSource == audioClip)
                {
                    stack++;
                }
            }

            if (stack > StackingAudioLimiter)
            {
                foreach (AudioReference s in audioReferences)
                {
                    if (s.audioSource == audioClip)
                    {
                        s.audioSource.transform.SetParent(audioPoolContainer);
                        audioPool.Enqueue(s.audioSource);

                        var audioSource = s.audioSource.GetComponent<AudioSource>();

                        audioSource.Stop();
                        
                        if (s.clipLength != null)
                        {
                            StopCoroutine(s.clipLength);
                        }
                        audioReferences.Remove(s);

                        return;
                    }
                }
            }
        }

        private void PopulateTheAudioSource(AudioScriptableObject sound, GameObject gameObject, GameObject obj, AudioClip audioClip, AudioSource audioSource)
        {
            audioSource.clip = audioClip;

            foreach (var clip in sound.audioClips)
            {
                if (clip.obj == audioSource.clip)
                {
                    //check if a high priority sound has been played.
                    if (activeAudioPriority > 0 && sound.audioPriority < activeAudioPriority)
                    {
                        audioSource.volume = clip.volume * reductionAmount;
                    }
                    else
                    {
                        audioSource.volume = clip.volume;
                    }

                    audioSource.pitch = clip.pitch;
                }
            }

            audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
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
        }

        private void CreateAudioReference(AudioScriptableObject sound, GameObject gameObject, GameObject obj, AudioSource audioSource)
        {
            var createdObjReference = new AudioReference();

            createdObjReference.objReference = sound;
            createdObjReference.requestingObj = gameObject;
            createdObjReference.audioSource = obj;
            createdObjReference.volume = audioSource.volume;

            Coroutine clipLength = null;

            if (!audioSource.loop)
            {
                clipLength = StartCoroutine(Countdown(audioSource.clip.length, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            createdObjReference.endOfClip = new UnityEvent();

            audioReferences.Add(createdObjReference);
        }

        #endregion










    }
}
