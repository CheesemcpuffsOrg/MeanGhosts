using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ItemInfo
{
    public ItemScriptableObject itemScriptableObject;
    public int amount;
}

//make a list of the totems whenever one is picked up, make this it's own class, pull from this list then map to the remove item call
//https://forum.unity.com/threads/custom-events-and-non-monobehaviour-class.225774/
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager InventoryManagerInstance;

    public event Action<ItemInfo> ItemAddedEvent;
    public event Action<ItemInfo> ItemRemovedEvent;

    private Dictionary<string, ItemInfo> itemDictionary = new Dictionary<string, ItemInfo>();
    List<ItemInfo> totems = new List<ItemInfo>();

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

    public void RemoveTotemItem()
    {

    }
}
