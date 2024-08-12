using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISoundComponent
{
    public void PlaySound(AudioScriptableObject audioScriptableObject, UnityAction fireEventWhenSoundFinished = null);

    public void StopSound(AudioScriptableObject audioScriptableObject);

    public bool IsSoundPlaying(AudioScriptableObject audioScriptableObject);

    public void DynamicVolumePrioritySystem(AudioScriptableObject audioScriptableObject, bool systemIsActive);

    public void DelayedPlaySound(float delay, AudioScriptableObject sound);
}
