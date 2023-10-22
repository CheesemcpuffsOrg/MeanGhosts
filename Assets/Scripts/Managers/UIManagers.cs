using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManagers : MonoBehaviour
{
    public static UIManagers UIManagersInstance;
    
    [SerializeField] TMP_Text lampText, pickUpItemText, placeItemText, playTapeText;
    TMP_Text graveNameText;
    Image itemImage;

    private void Awake()
    {
        UIManagersInstance = this;
    }

    public void DisableLampText()
    {
        if (lampText.enabled == true)
        {
            lampText.enabled = false;
        }
    }  

    public void GraveName(string name)
    {
        graveNameText = this.transform.Find(name).GetComponent<TMP_Text>();

        if (graveNameText.enabled == true)
        {
            graveNameText.enabled = false;
        }
        else
        {
            graveNameText.enabled = true;
        }  
    }

    public void PickUpItem()
    {
        if (pickUpItemText.enabled == true)
        {
            pickUpItemText.enabled = false;
        }
        else
        {
            pickUpItemText.enabled = true;
        }
    }

    public void PlaceItemShowText()
    {

        placeItemText.enabled = true;

    }

    public void PlaceItemHideText()
    {
       
        placeItemText.enabled = false;
        
    }

    public void PlayTape()
    {
        if (playTapeText.enabled == true)
        {
            playTapeText.enabled = false;
        }
        else
        {
            playTapeText.enabled = true;
        }
    }

    public void EnableItemImage(string name)
    {
        itemImage = this.transform.Find(name).GetComponent<Image>();

        itemImage.enabled = true;

    }

    public void DisableItemImage()
    {
        itemImage.enabled = false;
    }

}
