
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] GameObject soundComponentObj;
    ISoundComponent soundComponent;

    public static LevelManager levelManagerInstance;

    [Header ("Sounds")]
    [SerializeField] AudioScriptableObject bgSound;

    private void Awake()
    {
        levelManagerInstance = this;
    }

    void Start()
    {
        soundComponent = soundComponentObj.GetComponent<ISoundComponent>();

        soundComponent.PlaySound(bgSound, transform.position);

    }
}
