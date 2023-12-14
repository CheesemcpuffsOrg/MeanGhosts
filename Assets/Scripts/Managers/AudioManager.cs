using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Joshua 2023/12/14

namespace AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        private class AudioReference
        {
            public string name;
            public GameObject obj;
            public AudioSource audioSource;
        }

        [System.Serializable]
        private class TestAudioReference
        {
            public string name;
            public GameObject requestingObj;
            public GameObject audioSource;
            public Coroutine clipLength;
        }

        public static AudioManager AudioManagerInstance;

        private AudioScriptableObject[] sounds;

        private List<AudioScriptableObject> arrayStorage = new List<AudioScriptableObject>();
        private List<AudioSource> audioStorage = new List<AudioSource>();
        //this has to stay serialized because fucking unity won't let me assign the size at start #coconut.jpg
        [SerializeField]private List<AudioReference> audioReference = new List<AudioReference>();
        private List<AudioReference> spatialAudioSources = new List<AudioReference>();


        [SerializeField] GameObject audioObj;
        [SerializeField]List<TestAudioReference> testAudioReference = new List<TestAudioReference>();
        [SerializeField]Queue<GameObject> audioPool = new Queue<GameObject>();

        [SerializeField] Transform audioPoolContainer;
        [SerializeField] Transform activeSounds;


        private void Awake()
        {
            AudioManagerInstance = this;

            PopulateAudioManager();

        }

        private void Start()
        {
            sounds = arrayStorage.ToArray();//store all the list values to an array
        }

        /// <summary>
        /// This needs to be run on start. Make sure to attach all relevant audio disks to the audio manager.
        /// </summary>
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

        public void TestPlaySound(string name, GameObject gameObject)
        {
            AudioScriptableObject s = Array.Find(sounds, sound => sound.name == name);

            AudioSource audioSource;
            GameObject obj;
            Coroutine clipLength = null;

            if (audioPool.Count <= 0) 
            {
                obj = Instantiate(audioObj);

                audioSource = audioObj.GetComponent<AudioSource>();
            }
            else
            {
                obj = audioPool.Dequeue();

                audioSource = obj.GetComponent<AudioSource>();
            }

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

            Debug.Log(audioSource.spatialBlend);

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

            TestAudioReference createdObjReference = new TestAudioReference();

            createdObjReference.name = name;
            createdObjReference.requestingObj = gameObject;
            createdObjReference.audioSource = obj;

            if (!audioSource.loop)
            {
                clipLength = StartCoroutine(Countdown(audioSource.clip.length, createdObjReference));
            }

            createdObjReference.clipLength = clipLength;

            testAudioReference.Add(createdObjReference);

            audioSource.Play();
        }

        IEnumerator Countdown(float seconds, TestAudioReference reference)
        {
            yield return new WaitForSeconds(seconds);

            for (int i = 0; i <= testAudioReference.Count; i++)
            {
                if (testAudioReference[i].name == reference.name && testAudioReference[i].requestingObj == reference.requestingObj)
                {
                    testAudioReference[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(testAudioReference[i].audioSource);
                    testAudioReference.RemoveAt(i);
                    yield break;
                }
            }
        }

        public void TestStopSound(string name, GameObject gameObject)
        {
            for (int i = 0; i <= testAudioReference.Count; i++)
            {
                if (testAudioReference[i].name == name && testAudioReference[i].requestingObj == gameObject)
                {
                    testAudioReference[i].audioSource.transform.SetParent(audioPoolContainer);
                    audioPool.Enqueue(testAudioReference[i].audioSource);
                    testAudioReference.RemoveAt(i);
                    return;
                }
                else
                {
                    Debug.Log("Sound does not exist");
                }
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

        
    }

    


}
