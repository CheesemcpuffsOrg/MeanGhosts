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
        public GameObject slot;
        [HideInInspector] public Image Image;
        [HideInInspector] public ItemScriptableObject ItemRef;
    }

    [SerializeField] List<InventorySlot> slots = new List<InventorySlot>();
    bool started = false;

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
                slot.slot.SetActive(true);
                if(slot.Image == null) { slot.Image = slot.slot.GetComponent<Image>(); }
                slot.Image.sprite = item.inventorySprite;
                slot.ItemRef = item;
                return;
            }
        }
    }

    //Remove an item from the UI based off the item info
    void ItemRemoved(ItemInfo itemInfo)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.Image.sprite != null && slot.ItemRef == itemInfo.itemScriptableObject)
            {
                slot.slot.SetActive(false);
                slot.Image.sprite = null;
                slot.ItemRef = null;
                return;
            }
        }
    }

    void OnStartOrEnable()
    {
        InventoryManager.InventoryManagerInstance.ItemAddedEvent += ItemAdded;
        InventoryManager.InventoryManagerInstance.ItemRemovedEvent += ItemRemoved;
    }

    private void OnEnable()
    {
        if(started) { OnStartOrEnable(); }
    }

    private void OnDisable()
    {
        InventoryManager.InventoryManagerInstance.ItemAddedEvent -= ItemAdded;
        InventoryManager.InventoryManagerInstance.ItemRemovedEvent -= ItemRemoved;
    }
}
