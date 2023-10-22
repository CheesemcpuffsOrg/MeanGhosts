using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTape : InteractableObjs
{
    protected override void Interact()
    {
        base.Interact();

        this.gameObject.GetComponent<SoundController>().CheckIfPlaying(0);
        this.gameObject.GetComponent<SoundController>().PlayOneShotSound(0);
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        UIManagers.UIManagersInstance.PlayTape();
    }

    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        UIManagers.UIManagersInstance.PlayTape();
    }
}
