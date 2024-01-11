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
        SortPlayers();
    }

    public static void SwapPlayer(int playerPosition)
    {
        if (playerPosition < players.Count)
        {
            activePlayer = players[playerPosition];
            SortPlayers();
        }
    }

    private static void SortPlayers()
    {
        List<PlayerController> newPlayerList = new List<PlayerController>();
        newPlayerList.Add(activePlayer);
        players.Remove(activePlayer);
        while (players.Count > 0)
        {
            int nextPlayerIndex = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].playerSortOrder < players[nextPlayerIndex].playerSortOrder)
                {
                    nextPlayerIndex = i;
                }
            }
            newPlayerList.Add(players[nextPlayerIndex]);
            players.RemoveAt(nextPlayerIndex);
        }
        players = newPlayerList;
    }
}
