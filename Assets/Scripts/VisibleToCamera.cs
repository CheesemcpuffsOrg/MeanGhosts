using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleToCamera : MonoBehaviour
{

    [Tooltip("Define the padding of the screen bounds")]
    [SerializeField] Vector2 screenPadding = new Vector2(0, 1);

    [SerializeField] Transform objectToTrack;

    Vector3 viewPos;
    Camera cam;

    bool isVisible;

    public bool IsVisible => isVisible;

    [Header("References")]
    [SerializeField] ReferenceScriptableObject cameraReference;

    // Start is called before the first frame update
    void Start()
    {
        var cameraObject = ReferenceManager.ReferenceManagerInstance.GetReference(cameraReference);
        cam = cameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        isVisible = IsVisibleToCamera();  
    }

    private bool IsVisibleToCamera()
    {
        viewPos = cam.WorldToViewportPoint(objectToTrack.position);

        if (viewPos.x < screenPadding.y && viewPos.x > screenPadding.x && viewPos.y < screenPadding.y && viewPos.y > screenPadding.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
