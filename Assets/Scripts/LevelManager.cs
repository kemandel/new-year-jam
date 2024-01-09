using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameSettings defaultSettings;
    public static GameSettings Settings {get; private set;}
    public static PlayerController activePlayer {get; private set;}
    public PlayerController startingPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Settings = defaultSettings;
        activePlayer = startingPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
