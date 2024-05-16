using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public event Action<ItemInfo> ItemAddedEvent;
    public event Action<ItemInfo> ItemRemovedEvent;

    private Dictionary<string, ItemInfo> itemDictionary = new Dictionary<string, ItemInfo>();

    private void Awake()
    {
        InventoryManagerInstance = this;
    }

    public void AddItem(ItemInfo itemToAdd)
    {
        var itemName = itemToAdd.itemScriptableObject.name;

        if (itemDictionary.TryGetValue(itemName, out ItemInfo existingItem))
        {
            existingItem.amount += itemToAdd.amount;
            ItemAddedEvent?.Invoke(itemToAdd);
            return;
        }

        itemDictionary.Add(itemName, itemToAdd);
        ItemAddedEvent?.Invoke(itemToAdd);
        var item = itemDictionary.Values.First();
        Debug.Log(item.itemScriptableObject.name);
    }

    public void RemoveItem(ItemInfo itemToRemove)
    {
        if (itemDictionary.TryGetValue(itemToRemove.itemScriptableObject.name, out ItemInfo existingItem))
        {
            existingItem.amount -= itemToRemove.amount;
            ItemRemovedEvent?.Invoke(itemToRemove);
            return;
        }
        return;
    }
}
