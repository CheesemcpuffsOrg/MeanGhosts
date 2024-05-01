using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string name;

    public GameObject itemPrefab;
}
