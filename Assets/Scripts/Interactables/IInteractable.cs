using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable {

    /// <summary>
    /// Calls interact functionality when interact key is pressed
    /// </summary>
    public void Interact();

    public void Collision(bool hasCollided);

}
