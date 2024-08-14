using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFollowTransform : MonoBehaviour
{
    private Transform transformToFollow;

    public void AssignTransform(Transform transformToFollow)
    {
        this.transformToFollow = transformToFollow;
    }

    private void LateUpdate()
    {
        if (transformToFollow != null)
        {
            transform.position = transformToFollow.position;
        }
    }
}
