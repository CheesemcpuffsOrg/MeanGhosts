using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldTotems : MonoBehaviour
{
    private List<ItemInfo> heldTotems = new List<ItemInfo>();

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        started = true;
        OnStartOrEnable();
    }

    private void TotemAdded(ItemInfo itemInfo)
    {
        if(itemInfo.itemScriptableObject.type == ItemType.Totem)
        {
            heldTotems.Add(itemInfo);
        }
    }

    public void RemoveFirstTotem()
    {
        if(heldTotems.Count == 0)
        {
            return;
        }

        InventoryManager.InventoryManagerInstance.RemoveItem(heldTotems[0]);
        heldTotems.RemoveAt(0);
    }

    void OnStartOrEnable()
    {
        InventoryManager.InventoryManagerInstance.ItemAddedEvent += TotemAdded;
    }

    private void OnEnable()
    {
        if (started) { OnStartOrEnable(); }
    }

    private void OnDisable()
    {
        InventoryManager.InventoryManagerInstance.ItemAddedEvent -= TotemAdded;
    }
}
