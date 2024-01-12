using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public delegate void PlayerEvent(string characterName);

public class LevelManager : MonoBehaviour
{
    public static event PlayerEvent AddPlayerEvent;
    public GameSettings defaultSettings;
    public static GameSettings Settings {get; private set;}
    public static PlayerController activePlayer {get; private set;}
    public static List<PlayerController> players {get; set;}
    public PlayerController startingPlayer;
    private UIManager swapUIManager;
    // Start is called before the first frame update
    void Awake()
    {
        Settings = defaultSettings;
        players = new List<PlayerController>();

        activePlayer = startingPlayer;
        players.Add(activePlayer);
        swapUIManager = FindObjectOfType<UIManager>();
    }

    void LateUpdate()
    {
        PlayerController.playerInputRecorded = false;
    }

    public static void AddPlayer(PlayerController player)
    {
        players[players.Count-1].TrailingPlayer = player;
        players.Add(player);
        SortPlayers();
        AddPlayerEvent?.Invoke(player.characterName);

        //need to call swapUIManager.SetPlayer() but i cant bc it is static 
    }

    public static bool CheckForCharacter(string characterName)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].characterName == characterName)
            {
                return true;
            }
        }
        return false;
    }

    public static void SwapPlayer(int playerPosition)
    {
        if (playerPosition < players.Count)
        {
            Debug.Log("Swapping to " + players[playerPosition].name);

            activePlayer = players[playerPosition];
            SortPlayers();
        }
    }

    private static void SortPlayers()
    {
        List<PlayerController> newPlayerList = new List<PlayerController>();
        Vector3[] oldPositions = new Vector3[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            oldPositions[i] = players[i].transform.position;
        }

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
            newPlayerList[^2].TrailingPlayer = newPlayerList[^1];
        }
        newPlayerList[^1].TrailingPlayer = null;
        players = newPlayerList;

        // Swap player positions
        for (int i = 0; i < newPlayerList.Count; i++)
        {
            players[i].transform.position = oldPositions[i];
            players[i].CurrentNode = FindObjectOfType<C_Grid>().NodeFromWorldPoint(oldPositions[i]);
        }

        // Point active player forward
        if (players[0].TrailingPlayer != null)
            players[0].Rotate(new Vector2Int(Mathf.RoundToInt(players[0].transform.position.x - players[0].TrailingPlayer.transform.position.x), Mathf.RoundToInt(players[0].transform.position.y - players[0].TrailingPlayer.transform.position.y)));

        // Point non-active players forward
        for (int i = 0; i < players.Count - 1; i++)
        {
            players[i].TrailingPlayer.Rotate(new Vector2Int(Mathf.RoundToInt(players[i].transform.position.x - players[i].TrailingPlayer.transform.position.x), Mathf.RoundToInt(players[i].transform.position.y - players[i].TrailingPlayer.transform.position.y)));
        }

        foreach(PlayerController p in players)
        {
            p.InputEnabled = false;
        }
        players[0].InputEnabled = true;
    }
}
