using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{

    Dictionary<ReferenceScriptableObject, GameObject> references = new Dictionary<ReferenceScriptableObject, GameObject>();

    public static ReferenceManager ReferenceManagerInstance;

    private void Awake()
    {
        ReferenceManagerInstance = this;
    }

    public void SetReference(ReferenceScriptableObject reference, GameObject obj)
    {
        
        references.Add(reference, gameObject);
    }

    public GameObject GetReference(ReferenceScriptableObject reference)
    {
        if (references.ContainsKey(reference))
        {
            return references[reference];
        }

        return null;
    }
}
