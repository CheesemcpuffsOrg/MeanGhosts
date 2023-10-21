using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampedRotation : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = transform.localEulerAngles;
        if(euler.z > 270)
        {
            euler.z = euler.z - 360;
        }
        euler.z = Mathf.Clamp(euler.z, 45, 135); //-45, 45 / 65, 115 / -115, -65 /
        transform.localEulerAngles = euler;
    }
}
