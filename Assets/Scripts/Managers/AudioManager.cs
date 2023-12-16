using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//Joshua 2023/12/14

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        /*private class AudioReference
        {
            public string name;
            public GameObject obj;
            public AudioSource audioSource;
        }*/

        private class AudioReference
        {
            public string name;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
        }

        public static AudioManager AudioManagerInstance;

        private AudioScriptableObject[] sounds;

        [SerializeField]private List<AudioScriptableObject> arrayStorage = new List<AudioScriptableObject>();
        //private List<AudioSource> audioStorage = new List<AudioSource>();
        //this has to stay serialized because fucking unity won't let me assign the size at start #coconut.jpg
        /*[SerializeField] private List<AudioReference> audioReference = new List<AudioReference>();
        private List<AudioReference> spatialAudioSources = new List<AudioReference>();*/

        [SerializeField] GameObject audioObj;
        List<AudioReference> audioReference = new List<AudioReference>();
        Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;

        private void Awake()
        {
            AudioManagerInstance = this;

            //PopulateAudioManager();
        }

        private void Start()
        {
            sounds = arrayStorage.ToArray();//call this on start
        }

        /// <summary>
        /// This needs to be run on start. Make sure to attach all relevant audio disks to the audio manager.
        /// </summary>
        /*void PopulateAudioManager()
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
        }*/

        /// <summary>
        /// Stores scriptable objects on the audio manager for easier access
        /// </summary>
        public void GenerateAudioList(AudioScriptableObject[] audioList)
        {
            arrayStorage.AddRange(audioList);//add all the array values to a list
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
        public void PlaySound(string soundName, GameObject gameObject)
        {
            AudioScriptableObject s = Array.Find(sounds, sound => sound.name == soundName);

            AudioSource audioSource = null;
            GameObject obj = null;
            Coroutine clipLength = null;

            if (audioPool.Count < 5)
            {
                obj = Instantiate(audioObj);
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
            for (int i = 0; i <= audioReference.Count; i++)
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
            Debug.Log("Sound does not exist");
        }

        /// <summary>
        /// Allows you to delay the activation of a sound.
        /// </summary>
        public IEnumerator DelayedPlaySound(float delay, string name, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);

            PlaySound(name, gameObject);
        }


        /* /// <summary>
         /// Play sound by calling scriptable object name
         /// </summary>
         /// <param name="name">Scriptable object name</param>
         /// <returns>Int to track audio source being used</returns>
         public void PlaySound(string name, GameObject gameObject)
         {
             AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

             for (int i = 0; i <= audioStorage.Count; i++)
             {
                 AudioSource audioSource = audioStorage[i];

                 string audioCurrentNameReference = audioReference[i].name;
                 GameObject audioCurrentObjReference = audioReference[i].obj;

                 if (audioSource.isPlaying == false && audioCurrentNameReference == name && audioCurrentObjReference == gameObject)
                 {
                     audioSource.Play();
                     return;
                 }
                 else if (audioSource.isPlaying == false && audioCurrentNameReference != name)
                 {
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
                     audioSource.Play();

                     audioReference[i].name = name;
                     audioReference[i].obj = gameObject;

                     return;
                 } 
             }
             return;
         }

         /// <summary>
         /// Stop a sound that is playing
         /// </summary>
         /// <param name="arrayNumber">Number returned from play sound to stop the audio source</param>
         public void StopSound(string name, GameObject gameObject)
         {

             for (int i = 0; i <= audioStorage.Count; i++)
             {
                 AudioSource audioSource = audioStorage[i];

                 if (audioReference[i].name == name && audioReference[i].obj == gameObject)
                 {
                     audioSource.Stop();
                     return;
                 }
             }
             return;


         }

         /// <summary>
         /// Play oneshot by calling scriptable object name
         /// </summary>
         /// <param name="name">Scriptable object name</param>
         public void PlayOneShotSound(string name, GameObject gameObject)
         {

             AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

             for (int i = 0; i <= audioStorage.Count; i++)
             {
                 AudioSource audioSource = audioStorage[i];

                 string audioCurrentNameReference = audioReference[i].name;
                 GameObject audioCurrentObjReference = audioReference[i].obj;

                 if (audioSource.isPlaying == false && audioCurrentNameReference == name && audioCurrentObjReference == gameObject)
                 {
                     audioSource.Play();
                     return;
                 }
                 else if (audioSource.isPlaying == false && audioCurrentNameReference != name)
                 {
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
                     audioSource.PlayOneShot(s.clip, s.volume);

                     audioReference[i].name = name;
                     audioReference[i].obj = gameObject;

                     return;
                 }
             }
             return;
         }

         /// <summary>
         /// Play sound by calling scriptable object name
         /// </summary>
         /// <param name="name">Scriptable object name</param>
         /// <returns>Int to track audio source being used</returns>
         public void PlaySoundAtLocation(string name, GameObject gameObject)
         {
             AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

             AudioSource audioSource = gameObject.AddComponent<AudioSource>();

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
             audioSource.Play();

             AudioReference audioReference = new AudioReference();

             audioReference.name = name;
             audioReference.obj = gameObject;
             audioReference.audioSource = audioSource;

             spatialAudioSources.Add(audioReference);
         }

         public void StopSoundAtLocation(string name, GameObject gameObject)
         {
             foreach (var obj in spatialAudioSources)
             {
                 if (obj.name == name && obj.obj == gameObject)
                 {
                     obj.audioSource.Stop();
                 }
             }
         }


    */
    }

    


}
