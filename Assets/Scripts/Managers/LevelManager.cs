using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSound());
    }

    IEnumerator StartSound()
    {
        yield return new WaitForSeconds(0.1f);
        AudioManager.AudioManagerInstance.PlaySound("BGMusic", this.gameObject);
    }
}
