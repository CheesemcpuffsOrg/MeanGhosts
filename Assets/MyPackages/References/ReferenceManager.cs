using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    [Serializable]
    private class PredefinedReference
    {
        [SerializeField] ReferenceScriptableObject reference;
        [SerializeField] GameObject gameObject;

        public ReferenceScriptableObject Reference => reference;
        public GameObject GameObject => gameObject;
    }

    Dictionary<ReferenceScriptableObject, GameObject> references = new Dictionary<ReferenceScriptableObject, GameObject>();

    public static ReferenceManager ReferenceManagerInstance;

    [SerializeField] List<PredefinedReference> predefinedReferences;

    private void Awake()
    {
        ReferenceManagerInstance = this;

        if(predefinedReferences != null)
        {
            foreach (var predefinedReference in predefinedReferences)
            {
                references.Add(predefinedReference.Reference, predefinedReference.GameObject);
            }
        }
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

        Debug.LogError($"The following reference {reference} does not exist");
        return null;
    }
}
