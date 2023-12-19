using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua - 2023/12/15

namespace Interactable
{
    public class PlayTape : MonoBehaviour, IInteractable
    {
        [SerializeField] SoundDisk soundDisk;

        [SerializeField] AudioScriptableObject audioScriptableObject;

        void IInteractable.Interact()
        {
            AudioManager.AudioManagerInstance.PlaySound(audioScriptableObject, soundDisk, this.gameObject);
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

