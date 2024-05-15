using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct ItemInfo
{
    public ItemScriptableObject itemScriptableObject;
    public int amount;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance;

    public event Action<ItemInfo> ItemAdded;
    public event Action<ItemInfo> ItemRemoved;

    private Dictionary<string, ItemInfo> itemDictionary = new Dictionary<string, ItemInfo>();

    void Start()
    {
        InventoryManagerInstance = this;
    }

    public void AddItem(ItemInfo itemToAdd)
    {
        if (itemDictionary.TryGetValue(itemToAdd.itemScriptableObject.name, out ItemInfo existingItem))
        {
            existingItem.amount += itemToAdd.amount;
            ItemAdded?.Invoke(itemToAdd);
            return;
        }

        itemDictionary.Add(itemToAdd.itemScriptableObject.name, itemToAdd);
        ItemAdded?.Invoke(itemToAdd);
    }

    public void RemoveItem(ItemInfo itemToRemove)
    {
        if (itemDictionary.TryGetValue(itemToRemove.itemScriptableObject.name, out ItemInfo existingItem))
        {
            existingItem.amount -= itemToRemove.amount;
            ItemRemoved?.Invoke(itemToRemove);
            return;
        }
        return;
    }
}
