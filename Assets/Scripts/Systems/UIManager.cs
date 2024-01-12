using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //private Canvas swapPlayerCanvas;
    public Image[] containerElems;
    private Sprite[] playerSprites;
    // Start is called before the first frame update
    void Start()
    {
        //swapPlayerCanvas = GameObject.FindGameObjectWithTag("SwapCanvas").GetComponent<Canvas>();
        playerSprites = Resources.LoadAll<Sprite>("PlayerSprites");
        //array of image elements containing 1-5 labels, always the same, just disabled at start
        //containerElems = swapPlayerCanvas.GetComponentsInChildren<Image>();
        foreach (Image container in containerElems)
        {
            container.gameObject.SetActive(false);
        }
        LevelManager.AddPlayerEvent += SetPlayerUI;
        //set Sam UI on
        SetPlayerUI("Sam");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPlayerUI(string playerName)
    {
        Sprite playerToSet = null;

        for (int i =0; i < playerSprites.Length; i++)
        {
            Debug.Log("looping through player sprites comparing " + playerName + "with" + playerSprites[i].name);
            if (playerName == playerSprites[i].name)
            {
                playerToSet = playerSprites[i];
            }
        }

        for (int i = 0; i < containerElems.Length; i++)
        {
            //find the next disabled containere in hierarchy and set to sprite and enable
            if (!containerElems[i].gameObject.activeInHierarchy)
            {
                //have to get the second image because container itself has an image component
                containerElems[i].GetComponentsInChildren<Image>()[1].sprite = playerToSet;
                containerElems[i].gameObject.SetActive(true);
                break;
            }
        }
    }

}

//get access to array of empty container sprites for characters where container already set 1-5 but sprite not set
//disable all but 1 (Sam) in start
//in Max's function FoundPlayer() call my UIfunciton
//UIfunction: check which player just got added, and find corresponding sprite (presumably from an array of sprites) 
//and for my array of containers, loop through until you find a disabled one, get its child sprite reneder component and set char sprite
//and enable
//if at any point dialogue is active, disable (can do this is dialogue script IG