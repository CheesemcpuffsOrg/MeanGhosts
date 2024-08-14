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
        

        [Serializable]
        private class AudioReference
        {
            public AudioScriptableObject scriptableObjectReference;
            public UniqueSoundID UUID;
            public GameObject audioSource;
            public AudioObjType type;
            public Coroutine clipLength;
            public UnityEvent endOfClip;
            public float volume;
        }

        enum AudioObjType { STATIC, FOLLOW }

        public static AudioManager AudioManagerInstance;

        List<AudioReference> audioReferences = new List<AudioReference>();
        Queue<GameObject> staticAudioPool = new Queue<GameObject>();
        Queue<GameObject> followAudioPool = new Queue<GameObject>();

        [SerializeField] GameObject staticAudioObjPrefab;
        [SerializeField] GameObject followAudioObjPrefab;

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        [SerializeField, Tooltip("This int controls the max number of one type of audio clip that can be played before the oldest audio clip is cancelled")] 
        int StackingAudioLimiter = 5;

        int activeAudioPriority = 0;
        float reductionAmount = 0.5f;

        private void Awake()
        {
            AudioManagerInstance = this;
        }

        #region -- PUBLIC METHODS --

        //I THINK YOU SHOULD ALWAYS DEFINE AN AUDIOS LOCATION, TO ALLOW FOR SEAMLESS SWITCH BETWEEN 3D AND 2D


        /// <summary>
        /// Play a sound.
        /// </summary>
        public void PlaySound(AudioScriptableObject sound, UniqueSoundID UUID)
        {
            if(!BasePlaySound(sound, UUID, AudioObjType.STATIC))
            {
                return;
            }
        }

        /// <summary>
        /// Play a sound and fire an event when the sound finishes or is stopped.
        /// </summary>
        public void PlaySound(AudioScriptableObject sound, UniqueSoundID UUID, UnityAction fireEventWhenSoundFinished = null)
        {
            var soundPlayedSuccessfully = BasePlaySound(sound, UUID, AudioObjType.STATIC);

            if (!soundPlayedSuccessfully)
            {
                return;
            }

            if (fireEventWhenSoundFinished != null)
            {
                FireEventWhenSoundFinished(sound, UUID, fireEventWhenSoundFinished);
            }
        }

        /// <summary>
        /// Play a sound and position the audio source at a transforms location.
        /// </summary>
        public void PlaySound(AudioScriptableObject sound, UniqueSoundID UUID, Transform playLocation, bool followTransform = false)
        {
            var type = AudioObjType.STATIC;

            if (followTransform)
            {
                type = AudioObjType.FOLLOW;
            }

            var soundPlayedSuccessfully = BasePlaySound(sound, UUID, type);

            if (!soundPlayedSuccessfully)
            {
                return;
            }

            PositionAudioSourceAtTransform(sound, UUID, playLocation, followTransform);
        }

        /// <summary>
        /// Stops an active sound.
        /// </summary>
        public void StopSound(AudioScriptableObject sound, UniqueSoundID UUID)
        {
            foreach (var audioReference in audioReferences)
            {
                if (audioReference.scriptableObjectReference == sound && audioReference.UUID.soundID == UUID.soundID)
                {
                    audioReference.audioSource.transform.SetParent(audioPoolContainer);

                    if(audioReference.type == AudioObjType.STATIC)
                    {
                        staticAudioPool.Enqueue(audioReference.audioSource);
                    }
                    else
                    {
                        followAudioPool.Enqueue(audioReference.audioSource);
                    }
                    
                    var audioSource = audioReference.audioSource.GetComponent<AudioSource>();

                    if (sound.fadeOut)
                    {
                        StartCoroutine(FadeOut(audioSource, sound.fadeOutDuration));
                    }
                    else
                    {
                        audioSource.Stop();
                    }

                    if (audioReference.clipLength != null)
                    {
                        StopCoroutine(audioReference.clipLength);
                    }

                    if (audioReference.endOfClip != null)
                    {
                        audioReference.endOfClip.Invoke();
                    }

                    audioReferences.Remove(audioReference);

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
            foreach (AudioReference audioReference in audioReferences)
            {
                StopSound(audioReference.scriptableObjectReference, audioReference.UUID);
            }
        }


        /// <summary>
        /// Call this method to stop all audio that has been called from a UUID
        /// </summary>
        /// <param name="UUID"></param>
        public void StopAllAudioFromUniqueSoundID(UniqueSoundID UUID)
        {
            foreach(AudioReference audioReference in audioReferences)
            {
                if(audioReference.UUID.soundID == UUID.soundID)
                {
                    StopSound(audioReference.scriptableObjectReference, audioReference.UUID);
                }
            }
        }

        /// <summary>
        /// Call this method when you pause the game.
        /// </summary>
        public void PauseAllAudio()
        {
            foreach (AudioReference audioReference in audioReferences)
            {
                if (!audioReference.scriptableObjectReference.playWhilePaused)
                {
                    audioReference.audioSource.GetComponent<AudioSource>().Pause();
                }
            }
        }

        /// <summary>
        ///  Call this method when you unpause the game.
        /// </summary>
        public void UnPauseAllAudio()
        {
            foreach (AudioReference audioReference in audioReferences)
            {
                if (!audioReference.scriptableObjectReference.playWhilePaused)
                {
                    audioReference.audioSource.GetComponent<AudioSource>().Play();
                }
            }
        }

        /// <summary>
        /// Allows you to delay the activation of a sound.
        /// </summary>
        public void DelayedPlaySound(float delay, AudioScriptableObject sound, UniqueSoundID UUID)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound scriptable object");
                return;
            }

            StartCoroutine(SoundDelay(delay, sound, UUID));
        }

        /// <summary>
        /// Checks to see if the requested sound exists and is playing.
        /// </summary>
        public bool IsSoundPlaying(AudioScriptableObject sound, UniqueSoundID UUID)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound scriptable object");
                return false;
            }

            foreach (var audioReference in audioReferences)
            {
                if (audioReference.scriptableObjectReference == sound && audioReference.UUID.soundID == UUID.soundID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The following method allows for dynamic control over the lowering and highering of all audios based on the priority of the SO parameter .
        /// </summary>
        public void DynamicVolumePrioritySystem(AudioScriptableObject sound, bool systemIsActive)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound scriptable object");
                return;
            }

            var fadeDuration = 1f;

            if (systemIsActive && sound.audioPriority >= activeAudioPriority)
            {
                activeAudioPriority = sound.audioPriority;

                foreach (var audioReference in audioReferences)
                {
                    if(audioReference.scriptableObjectReference.audioPriority < activeAudioPriority)
                    {
                        var audioSource = audioReference.audioSource.GetComponent<AudioSource>();
                        var volume = audioSource.volume;
                        StartCoroutine(FadeOut(audioSource, fadeDuration, (volume * reductionAmount)));
                    }
                }
            }
            else
            {
                foreach (var audioReference in audioReferences)
                {
                    if (audioReference.scriptableObjectReference.audioPriority < activeAudioPriority)
                    {
                        var audioSource = audioReference.audioSource.GetComponent<AudioSource>();
                        StartCoroutine(FadeIn(audioReference.audioSource.GetComponent<AudioSource>(), fadeDuration, audioReference.volume, audioSource.volume));
                    }
                }

                activeAudioPriority = 0;
            }
        }

        #endregion

        #region -- PRIVATE METHODS --

        /// <summary>
        /// Positions the audio source at the transform.  
        /// </summary>
        private void PositionAudioSourceAtTransform(AudioScriptableObject sound, UniqueSoundID UUID, Transform playLocation, bool followTransform)
        {
            var audioGameObject = audioReferences[audioReferences.Count - 1];

            if (audioGameObject != null && audioGameObject.scriptableObjectReference == sound && audioGameObject.UUID.soundID == UUID.soundID)
            {
                audioGameObject.audioSource.transform.position = playLocation.transform.position;
            }
            else
            {
                var reverseOrder = audioReferences;

                reverseOrder.Reverse();

                foreach (var audioReference in reverseOrder)
                {
                    if (audioReference.scriptableObjectReference == sound && audioReference.UUID.soundID == UUID.soundID)
                    {
                        audioGameObject.audioSource.transform.position = playLocation.transform.position;
                        break;
                    }
                }
            }

            if (followTransform)
            {
                audioGameObject.audioSource.GetComponent<AudioFollowTransform>().AssignTransform(playLocation);
                return;
            }

            Debug.LogError("Failed to position object at transform " + playLocation);
        }

        /// <summary>
        /// Fires an event when an audio source stops playing.
        /// </summary>
        private void FireEventWhenSoundFinished(AudioScriptableObject sound, UniqueSoundID UUID, UnityAction reference)
        {
            //reverse the list so it grabs the most recent sound that was added for tracking
            var reversableList = audioReferences;
            reversableList.Reverse();

            foreach (var audioReference in reversableList)
            {
                if (audioReference.scriptableObjectReference == sound && audioReference.UUID.soundID == UUID.soundID)
                {
                    audioReference.endOfClip.AddListener(reference);
                    return;
                }
            }
        }

        /// <summary>
        /// This will turn any non-looping audio into a oneshot
        /// </summary>
        IEnumerator Countdown(float seconds, AudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            foreach(var audioReference in audioReferences)
            {
                if (audioReference.scriptableObjectReference == reference.scriptableObjectReference && audioReference.UUID.soundID == reference.UUID.soundID)
                {
                    audioReference.audioSource.transform.SetParent(audioPoolContainer);

                    if(audioReference.endOfClip != null)
                    {
                        audioReference.endOfClip.Invoke();
                    }

                    if (audioReference.type == AudioObjType.STATIC)
                    {
                        staticAudioPool.Enqueue(audioReference.audioSource);
                    }
                    else
                    {
                        followAudioPool.Enqueue(audioReference.audioSource);
                    }

                    audioReferences.Remove(audioReference);

                    yield break;
                }
            }
        }

        private IEnumerator SoundDelay(float delay, AudioScriptableObject sound, UniqueSoundID UUID)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(sound, UUID);
        }

        /// <summary>
        /// Fade in audio source.
        /// </summary>
        private static IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume, float startingVolume = 0)
        {
            float currentTime = 0;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startingVolume, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// Fade out audio source.
        /// </summary>
        private static IEnumerator FadeOut(AudioSource audioSource, float duration, float targetVolume = 0)
        {
            var currentTime = 0f;
            var currentVolume = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(currentVolume, targetVolume, currentTime / duration);
                yield return null;
            }

            if(audioSource.volume <= 0)
            {
                audioSource.Stop();
            }
            
            yield break;
        }

        /// <summary>
        /// Base functionality required when calling a sound
        /// </summary>
        private bool BasePlaySound(AudioScriptableObject sound, UniqueSoundID UUID, AudioObjType type)
        {
            if (sound == null)
            {
                Debug.LogError($"You are missing a sound scriptable object");
                return false;
            }

            if (sound.singleInstanceAudio)
            {
                foreach (AudioReference reference in audioReferences)
                {
                    if (reference.scriptableObjectReference == sound)
                    {
                        return false;
                    }
                }
            }

            //priority system
            if (audioReferences.Count >= 32)
            {
                foreach (AudioReference reference in audioReferences)
                {
                    if (sound.audioPriority > reference.scriptableObjectReference.audioPriority)
                    {
                        StopSound(reference.scriptableObjectReference, reference.UUID);
                    }
                }
            }

            GameObject obj;
            
            if(type == AudioObjType.STATIC)
            {
                //take obj from audio pool, if there are none create a new object.
                if (staticAudioPool.Count > 0)
                {
                    obj = staticAudioPool.Dequeue();
                }
                else
                {
                    obj = Instantiate(staticAudioObjPrefab);
                }
            }
            else
            {
                //take obj from audio pool, if there are none create a new object.
                if (followAudioPool.Count > 0)
                {
                    obj = followAudioPool.Dequeue();
                }
                else
                {
                    obj = Instantiate(followAudioObjPrefab);
                }
            }

            var chosenAudioClip = RandomUtility.ObjectPoolCalculator(sound.audioClips);

            AudioFloodPrevention(chosenAudioClip);

            var audioSource = obj.GetComponent<AudioSource>();

            PopulateTheAudioSource(sound, UUID, obj, chosenAudioClip, audioSource);

            CreateAudioReference(sound, UUID, obj, audioSource, chosenAudioClip, type);

            audioSource.Play();

            var fadeIn = sound.fadeIn;
            var fadeInDuration = sound.fadeInDuration;

            if (fadeIn)
            {
                StartCoroutine(FadeIn(audioSource, fadeInDuration, audioSource.volume));
            }

            return true;
        }

        /// <summary>
        /// Prevent audio flooding by deleting the oldest audio source when the limit is exceeded.
        /// </summary>
        private void AudioFloodPrevention(AudioList audioListVariable)
        {
            var stack = 0;

            foreach (AudioReference s in audioReferences)
            {
                if (s.audioSource == audioListVariable.audioClip)
                {
                    stack++;
                }
            }

            if (stack > StackingAudioLimiter)
            {
                foreach (AudioReference s in audioReferences)
                {
                    if (s.audioSource == audioListVariable.audioClip)
                    {
                        StopSound(s.scriptableObjectReference, s.UUID);

                        return;
                    }
                }
            }
        }

        private void PopulateTheAudioSource(AudioScriptableObject sound, UniqueSoundID UUID, GameObject obj, AudioList audioListVariable, AudioSource audioSource)
        {
            audioSource.clip = audioListVariable.audioClip;
            
            //check if a high priority sound has been played.
            if (activeAudioPriority > 0 && sound.audioPriority < activeAudioPriority)
            {
                audioSource.volume = audioListVariable.volume * reductionAmount;
            }
            else
            {
                audioSource.volume = audioListVariable.volume;
            }

            audioSource.pitch = audioListVariable.pitch;
            audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
            audioSource.loop = sound.loop;
            audioSource.panStereo = sound.pan;
            audioSource.spatialBlend = sound.spatialBlend;
            audioSource.dopplerLevel = sound.dopplerLevel;
            audioSource.minDistance = sound.minDistance;
            audioSource.maxDistance = sound.maxDistance;
            audioSource.rolloffMode = sound.volumeRollOffMode;

            if (sound.volumeRollOffMode == AudioRolloffMode.Custom)
            {  
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, sound.volumeRollOffCurve);
            }


            if (audioSource.spatialBlend == 0)
            {
                obj.transform.SetParent(activeSounds);
                obj.transform.position = activeSounds.position;
            }
            else
            {
                //THIS NEEDS TO BE UPDATED SO THAT YOU CHOOSE WHETHER AN AUDIO SOURCE FOLLOWS OR NOT
                obj.transform.SetParent(gameObject.transform);
                obj.transform.position = gameObject.transform.position;
            }
        }

        private void CreateAudioReference(AudioScriptableObject sound, UniqueSoundID UUID, GameObject obj, AudioSource audioSource, AudioList audioListVariable, AudioObjType type)
        {
            var createdObjReference = new AudioReference();

            createdObjReference.scriptableObjectReference = sound;
            createdObjReference.UUID = UUID;
            createdObjReference.type = type;
            createdObjReference.audioSource = obj;
            createdObjReference.volume = audioListVariable.volume;

            Coroutine clipLength = null;

            if (!audioSource.loop)
            {
                var lengthCalculatingPitch = audioSource.clip.length / Math.Abs(audioListVariable.pitch);
                clipLength = StartCoroutine(Countdown(lengthCalculatingPitch, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            createdObjReference.endOfClip = new UnityEvent();

            audioReferences.Add(createdObjReference);
        }

        #endregion
    }
}
