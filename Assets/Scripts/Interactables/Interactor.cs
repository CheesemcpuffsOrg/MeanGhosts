using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua - 2023/11/22

namespace Interactable
{
    public class Interactor : MonoBehaviour
    {

        [SerializeField] PlayerController controller;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                controller.interactEvent.AddListener(collision.gameObject.GetComponent<IInteractable>().Interact);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                controller.interactEvent.AddListener(collision.gameObject.GetComponent<IInteractable>().Interact);
            }
        }
    }
}


