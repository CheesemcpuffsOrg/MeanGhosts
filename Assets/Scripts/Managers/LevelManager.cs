using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManagerInstance;

    [SerializeField] SoundDisk soundDisk;

    [SerializeField] AudioScriptableObject audioScriptableObject;

    private void Awake()
    {
        levelManagerInstance = this;
    }

    void Start()
    {
        AudioManager.AudioManagerInstance.DelayedPlaySound(0.1f, audioScriptableObject, soundDisk, this.gameObject);
    }
}
