using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanNoise : MonoBehaviour
{
    bool playSound = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playSound)
        {
            GetComponent<SoundController>().PlayOneShotSound(0);
            GetComponent<SoundController>().PlayOneShotSound(1);
            playSound = false;
        }
    }
}
