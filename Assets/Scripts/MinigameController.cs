using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public const string WALL_ID = "Wall";
    public const string PLAYER_ID = "Player";
    public const string GOAL_ID = "Goal";
    public const string MOVEABLE_ID = "Move";

    private GameObject[,] minigameObjects;
    private GameObject playerObject;
    private Vector2Int playerPosition;

    public static bool Active {get; private set;}

    public Vector2 gridOffset;

    private void Start()
    {
        Active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        minigameObjects = new GameObject[11,9];
    }

    private void Update()
    {
        if (!Active) return;

        // Check for inputs
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt((int)Input.GetAxisRaw("Vertical")));
        if (direction.x != 0)
        {
            
        }
        else if (direction.y != 0)
        {
            
        }
    }

    public void StartGame(TextAsset levelData)
    {
        Active = true;
        transform.position = LevelManager.activePlayer.transform.position;
        GetComponent<SpriteRenderer>().enabled = true;

        string levelText = levelData.text;
        string[] lines = levelText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '*')
                {
                    minigameObjects[j,i] = CreateObject(new Vector2Int(j, i), WALL_ID);
                }
                else if (lines[i][j] == 'G')
                {
                    minigameObjects[j,i] = CreateObject(new Vector2Int(j, i), GOAL_ID);
                }
                else if (lines[i][j] == 'M')
                {
                    minigameObjects[j,i] = CreateObject(new Vector2Int(j, i), MOVEABLE_ID);
                }
                else if (lines[i][j] == 'P')
                {
                    minigameObjects[j,i] = CreateObject(new Vector2Int(j, i), PLAYER_ID);
                    playerObject = minigameObjects[j,i];
                }
            }
        }
    }

    private GameObject CreateObject(Vector2Int objectlocation, string objectID)
    {
        GameObject objectToSpawn = Resources.Load<GameObject>("Minigame/Objects/" + objectID);
        Vector3 spawnPosition = (Vector2)transform.position + gridOffset + new Vector2(objectlocation.x * .5f, objectlocation.y * -.5f);
        return Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, transform);
    }
}
