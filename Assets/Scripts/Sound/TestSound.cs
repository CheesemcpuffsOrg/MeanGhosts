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
        this.gameObject.GetComponent<SoundController>().PlaySound(0);

        yield return new WaitForSeconds(3f);

        //StartCoroutine(PlaySound());
    }
}
