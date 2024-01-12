using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Vineplant : NodeObject, IInteractable
{
    public int vineLength = 3;
    public Sprite ungrownSprite;
    public Sprite grownSprite;
    public Tile vineTile;
    public Tile endTile;
    public Tile noVineEndTile;

    public Dialogue[] noFaeDialogue;
    public Dialogue[] useFaeDialogue;
    public Dialogue[] growDialogue;

    public bool startGrown;

    private bool grown = false;
    private bool retractable = false;

    public override void Start()
    {
        base.Start();
        GetComponent<BoxCollider2D>().offset = new Vector2(0, FindObjectOfType<C_Grid>().nodeRadius * 2 * (vineLength + 1));

        if (startGrown)
        {
            Grow();
        }
    }

    void Update()
    {
        if (retractable)
        {
            if (Vector2.Distance(transform.position, LevelManager.activePlayer.transform.position) > FindObjectOfType<C_Grid>().nodeRadius * 2 * LevelManager.players.Count && transform.position.y > LevelManager.activePlayer.transform.position.y)
            {
                Retract();
            }
            else if (Vector2.Distance(transform.position, LevelManager.activePlayer.transform.position) > FindObjectOfType<C_Grid>().nodeRadius * 2 * (vineLength + 2) && transform.position.y < LevelManager.activePlayer.transform.position.y)
            {
                retractable = false;
            }
        }
    }

    public void Interact()
    {
        if (!LevelManager.CheckForCharacter("Fae"))
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(noFaeDialogue);
            return;
        }

        if (LevelManager.activePlayer.characterName != "Fae")
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(useFaeDialogue);
            return;
        }

        StartCoroutine(GrowVineCoroutine());
    }

    private IEnumerator GrowVineCoroutine()
    {
        FindObjectOfType<DialogueSystem>().PlayDialogue(growDialogue);
        LevelManager.activePlayer.InputEnabled = false;
        while (DialogueSystem.Active)
        {
            yield return null;
        }

        Grow();

        LevelManager.activePlayer.InputEnabled = true;
    }

    private void Grow()
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
        grown = true;
    }

    private void Retract()
    {
        Tilemap tilemap = FindObjectOfType<C_Grid>().walkableMap;
        GetComponentInChildren<SpriteRenderer>().sprite = ungrownSprite;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
        for (int i = 0; i < vineLength; i++)
        {
            tilemap.SetTile(tilemap.WorldToCell(transform.position) + new Vector3Int(0, i + 1, 0), null);
        }
        tilemap.SetTile(tilemap.WorldToCell(transform.position) + new Vector3Int(0, vineLength + 1, 0), noVineEndTile);
        FindObjectOfType<C_Grid>().RefreshGridWalkable();
        grown = false;
        retractable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (grown)
            retractable = true;
    }
}
