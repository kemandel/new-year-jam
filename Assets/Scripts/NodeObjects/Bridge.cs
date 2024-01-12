using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bridge : NodeObject, IInteractable
{
    public GameObject bridgeStart;
    public GameObject bridgeEnd;

    public int bridgeLength;
    public Sprite startBuiltSprite;
    public Sprite endBuiltSprite;
    public Tile bridgeTile;

    public Dialogue[] noMabelDialogue;
    public Dialogue[] useMabelDialogue;
    public Dialogue[] buildDialogue;

    public override void Start()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(bridgeStart.transform.position).currentObject = gameObject;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(bridgeEnd.transform.position).currentObject = gameObject;
        bridgeEnd.transform.localPosition = new Vector2(FindObjectOfType<C_Grid>().nodeRadius * 2 * (bridgeLength + 1), 0);

    }

    public void Interact()
    {
        if (!LevelManager.CheckForCharacter("Mabel"))
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(noMabelDialogue);
            return;
        }

        if (LevelManager.activePlayer.characterName != "Mabel")
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(useMabelDialogue);
            return;
        }

        StartCoroutine(BuildBridgeCoroutine());
    }

    private IEnumerator BuildBridgeCoroutine()
    {
        FindObjectOfType<DialogueSystem>().PlayDialogue(buildDialogue);
        LevelManager.activePlayer.InputEnabled = false;
        while (DialogueSystem.Active)
        {
            yield return null;
        }

        // Fade to black

        BuildBridge();

        LevelManager.activePlayer.InputEnabled = true;
    }

    private void BuildBridge()
    {
        Tilemap tilemap = FindObjectOfType<C_Grid>().walkableMap;
        bridgeStart.GetComponent<SpriteRenderer>().sprite = startBuiltSprite;
        bridgeEnd.GetComponent<SpriteRenderer>().sprite = endBuiltSprite;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = null;
        for (int i = 0; i < bridgeLength; i++)
        {
            tilemap.SetTile(tilemap.WorldToCell(transform.position) + new Vector3Int(i + 1, 0, 0), bridgeTile);
        }
        FindObjectOfType<C_Grid>().RefreshGridWalkable();
    }
}
