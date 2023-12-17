using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{

    [SerializeField] SoundDisk soundDisk;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySound());


         AudioManager.AudioManagerInstance.DelayedPlaySound(3f, "Mirror", "Tapes", this.gameObject);
    }

    IEnumerator PlaySound()
    {

        yield return new WaitForSeconds(6f);

        AudioManager.AudioManagerInstance.StopSound("Mirror", this.gameObject);

        //StartCoroutine(PlaySound());

    }


}
