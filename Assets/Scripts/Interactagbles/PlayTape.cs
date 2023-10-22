using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTape : InteractableObjs
{
    public override void Interact()
    {
        base.Interact();

        this.gameObject.GetComponent<SoundController>().CheckIfPlaying(0);
        this.gameObject.GetComponent<SoundController>().PlayOneShotSound(0);
    }
}
