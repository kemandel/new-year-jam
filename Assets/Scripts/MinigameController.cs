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
    private TextAsset currentLevelData;

    private bool lockinput = false;

    public static bool Active { get; private set; }

    public Vector2 gridOffset;

    public AudioClip minigameMusic;
    public AudioClip victorySound;

    private void Awake()
    {
        Active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        minigameObjects = new GameObject[11, 9];
    }

    private void LateUpdate()
    {
        if (!Active) return;

        if (!Input.anyKeyDown || lockinput) return;

        if (Input.GetKeyDown(LevelManager.Settings.interactKey))
        {
            RestartGame();
            return;
        }
        
        // Check for inputs
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt((int)Input.GetAxisRaw("Vertical")));
        if (direction == Vector2Int.zero) return;

        Vector2Int newPosition = direction.x != 0 ? playerPosition + new Vector2Int(direction.x, 0) : playerPosition + new Vector2Int(0, -direction.y);
        // If new position is in the bounds
        if (newPosition.x >= 0 && newPosition.x < minigameObjects.GetLength(0) && newPosition.y >= 0 && newPosition.y < minigameObjects.GetLength(1))
        {
            // If new position is empty
            if (minigameObjects[newPosition.x, newPosition.y] == null)
            {
                // Move player to new position
                playerObject.transform.position = (Vector2)transform.position + gridOffset + new Vector2(newPosition.x * .5f, newPosition.y * -.5f);
                minigameObjects[playerPosition.x, playerPosition.y] = null;
                minigameObjects[newPosition.x, newPosition.y] = playerObject;
                playerPosition = newPosition;
            }
            // If new position is moveable
            else if (minigameObjects[newPosition.x, newPosition.y].name[0] == 'M')
            {
                GameObject moveableObject = minigameObjects[newPosition.x, newPosition.y];
                Vector2Int positionToMove = direction.x != 0 ? newPosition + new Vector2Int(direction.x, 0) : newPosition + new Vector2Int(0, -direction.y);
                // if new moveable position is empty
                if (positionToMove.x >= 0 && positionToMove.x < minigameObjects.GetLength(0) && positionToMove.y >= 0 && positionToMove.y < minigameObjects.GetLength(1) && minigameObjects[positionToMove.x, positionToMove.y] == null)
                {
                    // Move moveable to new position
                    moveableObject.transform.position = (Vector2)transform.position + gridOffset + new Vector2(positionToMove.x * .5f, positionToMove.y * -.5f);
                    minigameObjects[newPosition.x, newPosition.y] = null;
                    minigameObjects[positionToMove.x, positionToMove.y] = moveableObject;

                    // Move player to new position
                    playerObject.transform.position = (Vector2)transform.position + gridOffset + new Vector2(newPosition.x * .5f, newPosition.y * -.5f);
                    minigameObjects[playerPosition.x, playerPosition.y] = null;
                    minigameObjects[newPosition.x, newPosition.y] = playerObject;
                    playerPosition = newPosition;
                }
            }

            // Check nearby spaces for goal
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;

                    Vector2Int neighborPosition = i != 0 ? playerPosition + new Vector2Int(i, 0) : playerPosition + new Vector2Int(0, j);
                    // if neighbor is in bounds
                    if (neighborPosition.x >= 0 && neighborPosition.x < minigameObjects.GetLength(0) && neighborPosition.y >= 0 && neighborPosition.y < minigameObjects.GetLength(1))
                    {
                        // if neighbor is goal
                        if (minigameObjects[neighborPosition.x, neighborPosition.y] != null && minigameObjects[neighborPosition.x, neighborPosition.y].name[0] == 'G')
                        {
                            StartCoroutine(EndGameCoroutine());
                        }
                    }
                }
            }
        }
    }

    public void StartGame(TextAsset levelData)
    {
        Active = true;
        transform.position = LevelManager.activePlayer.transform.position;
        GetComponent<SpriteRenderer>().enabled = true;

        FindObjectOfType<SoundManager>().PlayMusic(minigameMusic, LevelManager.Settings.musicVolume * .5f);

        string levelText = levelData.text;
        string[] lines = levelText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '*')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), WALL_ID);
                }
                else if (lines[i][j] == 'G')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), GOAL_ID);
                }
                else if (lines[i][j] == 'M')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), MOVEABLE_ID);
                }
                else if (lines[i][j] == 'P')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), PLAYER_ID);
                    playerObject = minigameObjects[j, i];
                    playerPosition = new Vector2Int(j, i);
                }
            }
        }

        currentLevelData = levelData;
    }

    private void RestartGame()
    {
        foreach (GameObject g in minigameObjects)
        {
            Destroy(g);
        }

        string levelText = currentLevelData.text;
        string[] lines = levelText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '*')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), WALL_ID);
                }
                else if (lines[i][j] == 'G')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), GOAL_ID);
                }
                else if (lines[i][j] == 'M')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), MOVEABLE_ID);
                }
                else if (lines[i][j] == 'P')
                {
                    minigameObjects[j, i] = CreateObject(new Vector2Int(j, i), PLAYER_ID);
                    playerObject = minigameObjects[j, i];
                    playerPosition = new Vector2Int(j, i);
                }
            }
        }
    }

    private IEnumerator EndGameCoroutine()
    {
        lockinput = true;
        StartCoroutine(FindObjectOfType<SoundManager>().FadeMusicAudioCoroutine(0, 0));
        FindObjectOfType<SoundManager>().PlaySoundEffect(victorySound, LevelManager.Settings.musicVolume * .5f);
        yield return new WaitForSeconds(victorySound.length + .1f);
        Active = false;
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (GameObject g in minigameObjects)
        {
            Destroy(g);
        }
        currentLevelData = null;
        lockinput = false;
        FindObjectOfType<SoundManager>().PlayMusic(FindObjectOfType<LevelManager>().forestMusic, 0);
    }

    private GameObject CreateObject(Vector2Int objectlocation, string objectID)
    {
        GameObject objectToSpawn = Resources.Load<GameObject>("Minigame/Objects/" + objectID);
        Vector3 spawnPosition = (Vector2)transform.position + gridOffset + new Vector2(objectlocation.x * .5f, objectlocation.y * -.5f);
        return Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, transform);
    }
}
