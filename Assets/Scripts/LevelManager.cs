using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameSettings defaultSettings;
    public static GameSettings Settings {get; private set;}
    public static PlayerController activePlayer {get; private set;}
    public static List<PlayerController> players {get; set;}
    public PlayerController startingPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Settings = defaultSettings;
        players = new List<PlayerController>();

        activePlayer = startingPlayer;
        players.Add(activePlayer);
    }

    public static void AddPlayer(PlayerController player)
    {
        players[players.Count-1].trailingPlayer = player;
        players.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
