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
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    [Range(-1f, 1f)]
    public float pan;
    [Range(0f, 1f)]
    public float spatialBlend;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
