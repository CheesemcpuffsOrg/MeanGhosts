using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound());


       
    }

    IEnumerator PlaySound()
    {

        yield return new WaitForSeconds(3f);

      //  AudioManager.AudioManagerInstance.TestPlaySound("Owl", this.gameObject);

        Debug.Log("hello");

        

        //StartCoroutine(PlaySound());
        NextMethod();
    }

    void NextMethod()
    {
        Debug.Log("hello again");
    }
}
