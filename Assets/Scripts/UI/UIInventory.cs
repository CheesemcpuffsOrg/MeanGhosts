using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [Serializable]
    private class InventorySlot
    {
        public Image Image;
        public ItemScriptableObject ItemRef;
    }

    [SerializeField] List<InventorySlot> slots = new List<InventorySlot>();
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        started = true;
        OnStartOrEnable();
    }

    void ItemAdded(ItemInfo itemInfo)
    {
        var item = itemInfo.itemScriptableObject;

        foreach (InventorySlot slot in slots)
        {
            if(slot.Image.sprite == null)
            {
                slot.Image.enabled = true;
                slot.Image.sprite = item.inventorySprite;
                slot.ItemRef = item;
            }
        }
        return;
    }

    void ItemRemoved(ItemInfo itemInfo)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.Image.sprite != null && slot.ItemRef == itemInfo.itemScriptableObject)
            {
                slot.Image.enabled = false;
                slot.Image.sprite = null;
                slot.ItemRef = null;
            }
        }
    }

    void OnStartOrEnable()
    {
        InventoryManager.InventoryManagerInstance.ItemAdded += ItemAdded;
        InventoryManager.InventoryManagerInstance.ItemRemoved += ItemRemoved;
    }

    private void OnEnable()
    {
        if(started) { OnStartOrEnable(); }
    }

    private void OnDisable()
    {
        InventoryManager.InventoryManagerInstance.ItemAdded -= ItemAdded;
        InventoryManager.InventoryManagerInstance.ItemRemoved -= ItemRemoved;
    }
}
