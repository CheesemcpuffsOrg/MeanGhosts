using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Joshua - 2023/12/15

namespace Interactable
{
    public class TapeInteractableObj : InteractableObjs, IInteractable
    {
        [Space]
        [Space]
        [Space]
        [Space]

        [SerializeField] GameObject soundComponentObj;
        ISoundComponent soundComponent;

        [Header ("Sounds")]
        [SerializeField] AudioScriptableObject tapeAudio;

        [Header ("Collision Proxies")]
        [SerializeField] private Collision2DProxy audioRangeDetection;

        protected override void Start()
        {
            base.Start();

            audioRangeDetection.OnTriggerEnter2D_Action += AudioDetectionOnTriggerEnter2D;
            audioRangeDetection.OnTriggerExit2D_Action += AudioDetectionOnTriggerExit2D;
        }

        public void InteractionPrompt(bool hasCollided)
        {
            UIContainer.UIContainerInstance.PlayTape(hasCollided);
        }

        void IInteractable.Interact()
        {
            if(!soundComponent.IsSoundPlaying(tapeAudio))
            {
                soundComponent.PlaySound(tapeAudio, ReturnAudioToNormal);
                soundComponent.DynamicVolumePrioritySystem(tapeAudio, true);
            }
        }

        void ReturnAudioToNormal()
        {
            soundComponent.DynamicVolumePrioritySystem(tapeAudio, false);
        }

        private void AudioDetectionOnTriggerEnter2D(Collider2D other)
        {
            if(TagExtensions.HasTag(other.gameObject, playerColliderTag))
            {
                if (soundComponent.IsSoundPlaying(tapeAudio))
                {
                    soundComponent.DynamicVolumePrioritySystem(tapeAudio, true);
                }
            }
        }

        private void AudioDetectionOnTriggerExit2D(Collider2D other)
        {
            if (TagExtensions.HasTag(other.gameObject, playerColliderTag))
            {
                if (soundComponent.IsSoundPlaying(tapeAudio))
                {   
                    soundComponent .DynamicVolumePrioritySystem(tapeAudio, false);
                }
            }
        }
    }
}

