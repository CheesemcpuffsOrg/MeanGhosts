using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContainer : MonoBehaviour
{
    public static UIContainer UIContainerInstance;
    
    [SerializeField] TMP_Text lampText, pickUpItemText, placeItemText, pauseText, gameOver, winner, volume;


    [SerializeField]
    TMP_Text _playTapeText;

    TMP_Text graveNameText;
    Image itemImage;
    [SerializeField] GameObject restartButton, slider;

    private void Awake()
    {
        UIContainerInstance = this;
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

    public void PickUpItem(bool result)
    {
        if(pickUpItemText == null) 
        {
            return;
        }

        if (!result)
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

    public void PlayTape(bool result)
    {

        if(_playTapeText == null)
        {
            return;
        }

        if (!result)
        {
            _playTapeText.enabled = false;
        }
        else
        {
            _playTapeText.enabled = true;
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

    public void PauseText(bool isPaused)
    {

        if (!isPaused)
        {
            pauseText.enabled = false;

            slider.SetActive(false);
            volume.enabled = false;
        }
        else
        {
            pauseText.enabled = true;

            slider.SetActive(true);
            volume.enabled = true;
        }
    }

    public void GameOver()
    {
        gameOver.enabled = true;
        restartButton.SetActive(true);
    }

    public void Winner()
    {
        winner.enabled = true;
        float timer = GameManager.GameManagerInstance.timer;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        winner.text = "Good job! You defeated the ghosts.<br> Your time was: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        restartButton.SetActive(true);
    }

    public void ResetGame()
    {
        GameManager.GameManagerInstance.RestartGame();
    }

}
