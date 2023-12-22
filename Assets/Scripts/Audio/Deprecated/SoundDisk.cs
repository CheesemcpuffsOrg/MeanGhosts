using AudioSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDisk : MonoBehaviour
{
    [SerializeField] AudioScriptableObject[] sounds;

    public AudioScriptableObject FindSound(string soundName)
    {
        AudioScriptableObject s = Array.Find(sounds, sound => sound.name == soundName);

        return s;
    }

}
