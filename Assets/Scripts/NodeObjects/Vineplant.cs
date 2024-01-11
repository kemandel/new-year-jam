using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Vineplant : NodeObject, IInteractable
{
    public int vineLength = 3;
    public Sprite grownSprite;
    public Tile vineTile;
    public Tile endTile;

    public void Interact()
    {
        Tilemap tilemap = FindObjectOfType<C_Grid>().walkableMap;

        GetComponentInChildren<SpriteRenderer>().sprite = grownSprite;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = null;
        for (int i = 0; i < vineLength; i++)
        {
            tilemap.SetTile(tilemap.WorldToCell(transform.position) + new Vector3Int(0, i + 1, 0), vineTile);
        }
        tilemap.SetTile(tilemap.WorldToCell(transform.position) + new Vector3Int(0, vineLength + 1, 0), endTile);
        FindObjectOfType<C_Grid>().RefreshGridWalkable();
    }
}
