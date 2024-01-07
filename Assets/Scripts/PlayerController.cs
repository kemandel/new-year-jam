using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Node CurrentNode {get; set;}

    private bool moving;

    public void Start()
    {
        CurrentNode = C_Grid.instance.NodeFromWorldPoint(transform.position);
        moving = false;
    }
    
    public void Update()
    {

        // Dont check for inputs if moving
        if (moving) return;

        // Check for inputs
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt((int)Input.GetAxisRaw("Vertical")));
        if (direction.x != 0)
        {
            Node newNode = C_Grid.instance.grid[CurrentNode.gridX + direction.x,CurrentNode.gridY];
            if (newNode.walkable)
            {
                StartCoroutine(MoveCoroutine(newNode));
            }
        }
        else if (direction.y != 0)
        {
            Node newNode = C_Grid.instance.grid[CurrentNode.gridX,CurrentNode.gridY + direction.y];
            if (newNode.walkable)
            {
                StartCoroutine(MoveCoroutine(newNode));
            }
        }
    }

    // Move player to new node
    private IEnumerator MoveCoroutine(Node newNode)
    {
        moving = true;
        Node oldNode = CurrentNode;
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
        CurrentNode = newNode;
        moving = false;
    }
}
