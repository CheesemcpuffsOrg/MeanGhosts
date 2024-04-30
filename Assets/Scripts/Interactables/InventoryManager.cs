using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Item
{
    public string name;
    public int amount;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance;

    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    void Start()
    {
        InventoryManagerInstance = this;
    }

    public void AddItem(Item itemToAdd)
    {
        if (itemDictionary.TryGetValue(itemToAdd.name, out Item existingItem))
        {
            existingItem.amount += itemToAdd.amount;
            return;
        }

        itemDictionary.Add(itemToAdd.name, itemToAdd);
        
    }
}
