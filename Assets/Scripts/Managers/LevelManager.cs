using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager levelManagerInstance;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject bgSound;

    private void Awake()
    {
        levelManagerInstance = this;
    }

    void Start()
    {
        AudioManager.AudioManagerInstance.DelayedPlaySound(0.1f, bgSound, this.gameObject);
    }
}
