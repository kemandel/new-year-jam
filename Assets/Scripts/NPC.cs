using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public PlayerController playerToControl;

    public TextAsset minigameData;

    void Start()
    {
        //FindObjectOfType<MinigameController>().StartGame(minigameData);
    }
}
