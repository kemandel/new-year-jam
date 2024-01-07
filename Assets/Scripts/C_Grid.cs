using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class C_Grid : MonoBehaviour
{
    public float nodeRadius;
    public Vector2 gridWorldSize;
    public Tilemap collisionMap;
    public Tilemap walkableMap;

    public Node[,] grid;
    public bool debug;
    private int gridSizeX, gridSizeY;

    public static C_Grid instance;

    float nodeDiameter;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.up * (gridWorldSize.y / 2);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldpoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !collisionMap.HasTile(collisionMap.WorldToCell(worldpoint)) && walkableMap.HasTile(walkableMap.WorldToCell(worldpoint));

                grid[x, y] = new Node(walkable, worldpoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x + y == 0 || x == y)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (grid != null && debug)
        {
            foreach (Node n in grid)
            {
                if (n.walkable)
                {
                    Gizmos.color = new Color(0, 255, 0, .1f);
                }
                else
                {
                    Gizmos.color = new Color(255, 0, 0, .1f);
                }
                Gizmos.DrawCube(n.worldPosition, new Vector3(1, 1, 0) * (nodeDiameter - .1f));
            }
        }
    }

    public Node RandomNode()
    {
        return grid[Random.Range(0, gridSizeX), Random.Range(0, gridSizeY)];
    }
}
