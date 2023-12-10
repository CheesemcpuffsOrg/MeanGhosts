using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Joshua 2023/12/10

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager AudioManagerInstance;

        int maxAudioComponents = 32;

        private AudioScriptableObject[] sounds;

        private List<AudioScriptableObject> arrayStorage = new List<AudioScriptableObject>();
        private List<AudioSource> audioStorage = new List<AudioSource>();
        [SerializeField] private List<GameObject> audioReference;


        private void Awake()
        {
            audioReference = new List<GameObject>(maxAudioComponents);

            AudioManagerInstance = this;

            PopulateAudioManager();

        }

        private void Start()
        {
            sounds = arrayStorage.ToArray();//store all the list values to an array

            float addition = UnityEngine.Random.Range(0.1f, -0.1f);
        }

        /// <summary>
        /// This needs to be run on start. Make sure to attach all relevant audio disks to the audio manager.
        /// </summary>
        void PopulateAudioManager()
        {

            for (int i = 0; i < maxAudioComponents; i++)
            {
                this.gameObject.AddComponent<AudioSource>(); 
            }

            foreach (AudioSource s in GetComponents<AudioSource>())
            {
                s.GetComponents<AudioSource>();
                audioStorage.Add(s);
                audioReference.Add(null);
            }
        }

        /// <summary>
        /// Stores scriptable objects on the audio manager for easier access
        /// </summary>
        /// <param name="audioList">Store audio scriptable objects</param>
        public void GenerateAudioComponentList(AudioScriptableObject[] audioList)
        {
            arrayStorage.AddRange(audioList);//add all the array values to a list
        }

        /// <summary>
        /// Stops all audio
        /// </summary>
        public void StopAllAudio()
        {
            foreach (AudioSource s in audioStorage)
            {
                s.Stop();
            }
        }

        /// <summary>
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

                if (audioSource.isPlaying == false && audioSource.name != name)
                {
                    audioSource.clip = s.clip;

                    audioSource.outputAudioMixerGroup = s.group;
                    audioSource.volume = s.volume;
                    audioSource.pitch = s.pitch;
                    audioSource.loop = s.loop;
                    audioSource.panStereo = s.pan;
                    audioSource.spatialBlend = s.spatialBlend;
                    audioSource.Play();

                    audioReference[i] = gameObject;

                    return;
                }
                else if(audioSource.isPlaying == false && audioSource.name == name && audioReference[i] == gameObject)
                {
                    audioSource.Play();
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
            for(int i = 0; i <= audioStorage.Count; i++)
            {
                if (audioStorage[i].name == name && audioReference[i] == gameObject)
                {
                    audioStorage[i].Stop();
                    return;
                }  
            }
            return;
        }

        /// <summary>
        /// Play oneshot by calling scriptable object name
        /// </summary>
        /// <param name="name">Scriptable object name</param>
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
            return;
        }
    }
}
