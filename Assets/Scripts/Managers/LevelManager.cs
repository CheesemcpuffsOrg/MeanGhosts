using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManagerInstance;

    [SerializeField] SoundDisk soundDisk;

    private void Awake()
    {
        levelManagerInstance = this;
    }

    void Start()
    {
        AudioManager.AudioManagerInstance.DelayedPlaySound(0.1f, "BGMusic", "BackgroundMusic", this.gameObject);
    }
}
