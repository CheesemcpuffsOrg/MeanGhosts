using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio", menuName = "Audio")]
public class AudioScriptableObject : ScriptableObject
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup group;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;
    [Range(-1f, 1f)]
    public float pan = 0;
    [Range(0f, 1f)]
    public float spatialBlend;
    [Range(0f, 5f)]
    public float dopplerLevel = 0;

    public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
    public float minDistance = 1;
    public float maxDistance = 30;

    public bool loop;
    public bool playOnAwake = false;

    [HideInInspector]
    public AudioSource source;
}
