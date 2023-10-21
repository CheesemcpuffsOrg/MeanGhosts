using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    [SerializeField]GameObject torchLight;

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = new Vector2(mousePosition.x - torchLight.transform.position.x, mousePosition.y - torchLight.transform.root.position.y);

        

        torchLight.transform.right = direction;
    }
    
}
