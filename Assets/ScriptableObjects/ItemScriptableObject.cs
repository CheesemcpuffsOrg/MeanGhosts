using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { None, Totem }

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class ItemScriptableObject : ScriptableObject
{
    
    public string name;

    public ItemType type = ItemType.None;

    public GameObject itemPrefab;
    public Sprite inventorySprite;
}
