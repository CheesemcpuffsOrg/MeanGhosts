using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManagerInstance;

    private void Awake()
    {
        levelManagerInstance = this;
    }

    void Start()
    {
        StartCoroutine(AudioManager.AudioManagerInstance.DelayedPlaySound(0.1f, "BGMusic", this.gameObject));
    }
}
