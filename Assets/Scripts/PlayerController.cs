using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string characterName;

    public PlayerController TrailingPlayer {get; set;}

    public Node CurrentNode {get; set;}
    public bool InputEnabled {get; set;} = true;
    public int playerSortOrder;

    public static bool playerInputRecorded = false;

    private bool moving;
    private Animator animator;
    private C_Grid grid;
    
    private static Node interactNode;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        moving = false;
    }

    public void Start()
    {
        grid = FindObjectOfType<C_Grid>();
        CurrentNode = grid.NodeFromWorldPoint(transform.position);
        if (LevelManager.activePlayer == this)
            interactNode = grid.grid[CurrentNode.gridX,CurrentNode.gridY-1];
    }
    
    public void Update()
    {
        // Dont check for inputs if moving or not in control
        if (moving)
        {
            if (TrailingPlayer != null)
            {
                int sortingOrder = TrailingPlayer.transform.position.y > transform.position.y ? GetComponentInChildren<SpriteRenderer>().sortingOrder - 1 : GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
                TrailingPlayer.GetComponentInChildren<SpriteRenderer>().sortingOrder = sortingOrder;
            }
            return;
        }
        else
        {
            animator.SetInteger("XVel", 0);
            animator.SetInteger("YVel", 0);
        }
        
        if (LevelManager.activePlayer != this || playerInputRecorded) return;
        if (MinigameController.Active) return;
        if (DialogueSystem.Active) return;
        if (!InputEnabled) return;

        playerInputRecorded = true;

        // Check for player swap
        for (int i = 0; i < LevelManager.Settings.characterSwapKeys.Length; i++)
        {
            if (Input.GetKeyDown(LevelManager.Settings.characterSwapKeys[i]))
            {
                LevelManager.SwapPlayer(i);
                return;
            }
        }

        // Check for inputs
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt((int)Input.GetAxisRaw("Vertical")));
        if (direction.x != 0)
        {
            Node newNode = grid.grid[CurrentNode.gridX + direction.x,CurrentNode.gridY];
            if (newNode.walkable && newNode.currentObject == null)
            {
                StartCoroutine(MoveCoroutine(newNode));
                interactNode = grid.grid[CurrentNode.gridX + direction.x,CurrentNode.gridY];
            }
            else 
            {
                StartCoroutine(RotateCoroutine(direction));
                interactNode = grid.grid[CurrentNode.gridX + direction.x,CurrentNode.gridY];
            }
        }
        else if (direction.y != 0)
        {
            Node newNode = grid.grid[CurrentNode.gridX,CurrentNode.gridY + direction.y];
            if (newNode.walkable && newNode.currentObject == null)
            {
                StartCoroutine(MoveCoroutine(newNode));
                interactNode = grid.grid[CurrentNode.gridX,CurrentNode.gridY + direction.y];
            }
            else 
            {
                StartCoroutine(RotateCoroutine(direction));
                interactNode = grid.grid[CurrentNode.gridX,CurrentNode.gridY + direction.y];
            }
        }
        else if (Input.GetKeyDown(LevelManager.Settings.interactKey))
        {
            if (interactNode.currentObject != null && interactNode.currentObject.GetComponent<IInteractable>() != null)
            {
                interactNode.currentObject.GetComponent<IInteractable>().Interact();
            }
        }
    }

    public void MoveToNode(Node node)
    {
        StartCoroutine(MoveCoroutine(node));
    }

    public void Rotate(Vector2Int direction)
    {
        StartCoroutine(RotateCoroutine(direction));
    }

    private IEnumerator RotateCoroutine(Vector2Int direction)
    {
        moving = true;
        animator.SetInteger("XVel", direction.x);
        animator.SetInteger("YVel", direction.y);
        yield return null;
        moving = false;
    }

    // Move player to new node
    private IEnumerator MoveCoroutine(Node newNode)
    {
        moving = true;

        Node oldNode = CurrentNode;
        CurrentNode = newNode;

        if (TrailingPlayer != null)
        {
            TrailingPlayer.MoveToNode(oldNode);
        }

        animator.SetInteger("XVel", Mathf.RoundToInt(newNode.worldPosition.x - oldNode.worldPosition.x));
        animator.SetInteger("YVel", Mathf.RoundToInt(newNode.worldPosition.y - oldNode.worldPosition.y));

        float moveTime = LevelManager.Settings.charMoveTime;
        float startTime = Time.time;
        float timePassed = 0;
        while (timePassed < moveTime)
        {
            float ratio = timePassed / moveTime;
            transform.position = Vector3.Lerp(oldNode.worldPosition, newNode.worldPosition, ratio);
            yield return null;
            timePassed = Time.time - startTime;
        }
        transform.position = newNode.worldPosition;
        moving = false;
    }
}
