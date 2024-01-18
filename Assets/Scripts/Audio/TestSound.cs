using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSound : MonoBehaviour
{
    [SerializeField] AudioScriptableObject audioScriptableObject;

    

    // Start is called before the first frame update
    void Start()
    {

        AudioManager.AudioManagerInstance.PlaySound(audioScriptableObject, this.gameObject);

        AudioManager.AudioManagerInstance.FireEventWhenSoundFinished(audioScriptableObject, this.gameObject, DoTheThing);

    }

    public void DoTheThing()
    {
        Debug.Log("Do the thing");
    }




}
