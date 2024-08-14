using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundComponent : MonoBehaviour, ISoundComponent
{

    UniqueSoundID UUID = new UniqueSoundID();

    public void PlaySound(AudioScriptableObject audioScriptableObject)
    {
        AudioManager.AudioManagerInstance.PlaySound(audioScriptableObject, UUID);
    }

    public void PlaySound(AudioScriptableObject audioScriptableObject, UnityAction fireEventWhenSoundFinished = null)
    {
        AudioManager.AudioManagerInstance.PlaySound(audioScriptableObject, UUID, fireEventWhenSoundFinished);
    }

    public void PlaySound(AudioScriptableObject audioScriptableObject, UniqueSoundID UUID, Transform playLocation, bool followTransform = false)
    {
        AudioManager.AudioManagerInstance.PlaySound(audioScriptableObject, UUID, playLocation, followTransform);
    }

    public void StopSound(AudioScriptableObject audioScriptableObject)
    {
        AudioManager.AudioManagerInstance.StopSound(audioScriptableObject, UUID);
    }

    public bool IsSoundPlaying(AudioScriptableObject audioScriptableObject)
    {
        return AudioManager.AudioManagerInstance.IsSoundPlaying(audioScriptableObject, UUID);
    }

    public void DynamicVolumePrioritySystem(AudioScriptableObject audioScriptableObject, bool systemIsActive)
    {
        AudioManager.AudioManagerInstance.DynamicVolumePrioritySystem(audioScriptableObject, systemIsActive);
    }

    public void DelayedPlaySound(float delay, AudioScriptableObject sound)
    {
        AudioManager.AudioManagerInstance.DelayedPlaySound(delay, sound, UUID);
    }
}
