using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua - 2023/11/22

namespace Interactable
{
    public class PlayTape : MonoBehaviour, IInteractable
    {

        void IInteractable.Interact()
        {
            this.gameObject.GetComponent<SoundController>().CheckIfPlaying(0);
            this.gameObject.GetComponent<SoundController>().PlayOneShotSound(0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            UIManagers.UIManagersInstance.PlayTape();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            UIManagers.UIManagersInstance.PlayTape();
        }
    }
}

