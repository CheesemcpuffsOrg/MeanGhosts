using AudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Joshua - 2023/12/15

namespace Interactable
{
    public class TapeInteractableObj : MonoBehaviour, IInteractable
    {
        TMP_Text playTapeText;

        [Header ("Sounds")]
        [SerializeField] AudioScriptableObject tapeAudio;

        [Header ("Collision Proxies")]
        [SerializeField] private Collision2DProxy audioRangeDetection;

        [Header ("Tags")]
        [SerializeField] TagScriptableObject playerColliderTag;

        private void Start()
        {
            playTapeText = UIContainer.UIContainerInstance.playTapeText;

            audioRangeDetection.OnTriggerEnter2D_Action += AudioDetectionOnTriggerEnter2D;
            audioRangeDetection.OnTriggerExit2D_Action += AudioDetectionOnTriggerExit2D;

        }

        public void InteractionPrompt(bool hasCollided)
        {
            if (hasCollided)
            {
                playTapeText.enabled = true;
            }
            else
            {
                if(playTapeText != null)//to prevent weird error when game is ended
                {
                    playTapeText.enabled = false;
                }
                
            }
        }

        void IInteractable.Interact()
        {
            if(!AudioManager.AudioManagerInstance.IsSoundPlaying(tapeAudio, this.gameObject))
            {
                AudioManager.AudioManagerInstance.PlaySound(tapeAudio, this.gameObject, ReturnAudioToNormal);
                AudioManager.AudioManagerInstance.DynamicVolumePrioritySystem(tapeAudio, true);
            }
        }

        void ReturnAudioToNormal()
        {
            AudioManager.AudioManagerInstance.DynamicVolumePrioritySystem(tapeAudio, false);
        }

        private void AudioDetectionOnTriggerEnter2D(Collider2D other)
        {
            if(TagExtensions.HasTag(other.gameObject, playerColliderTag))
            {
                if (AudioManager.AudioManagerInstance.IsSoundPlaying(tapeAudio, this.gameObject))
                {
                    AudioManager.AudioManagerInstance.DynamicVolumePrioritySystem(tapeAudio, true);
                }
            }
        }

        private void AudioDetectionOnTriggerExit2D(Collider2D other)
        {
            if (TagExtensions.HasTag(other.gameObject, playerColliderTag))
            {
                if (AudioManager.AudioManagerInstance.IsSoundPlaying(tapeAudio, this.gameObject))
                {   
                    AudioManager.AudioManagerInstance.DynamicVolumePrioritySystem(tapeAudio, false);
                }
            }
        }
    }
}

