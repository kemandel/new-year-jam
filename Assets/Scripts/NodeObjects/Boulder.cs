using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : NodeObject, IInteractable
{
    public Sprite destroyedSprite;

    public Dialogue[] noTeddyDialogue;
    public Dialogue[] useTeddyDialogue;
    public Dialogue[] destroyDialogue;

    // Start is called before the first frame update
    public override void Start()
    {
        C_Grid grid = FindObjectOfType<C_Grid>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                grid.NodeFromWorldPoint(transform.position + new Vector3(i * grid.nodeRadius * 2, j * grid.nodeRadius * 2)).currentObject = gameObject;
            }
        }
    }

    public void Interact()
    {
        if (!LevelManager.CheckForCharacter("Teddy"))
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(noTeddyDialogue);
            return;
        }

        if (LevelManager.activePlayer.characterName != "Teddy")
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(useTeddyDialogue);
            return;
        }

        StartCoroutine(DestroyBoulderCoroutine());
    }

    private IEnumerator DestroyBoulderCoroutine()
    {
        FindObjectOfType<DialogueSystem>().PlayDialogue(destroyDialogue);
        LevelManager.activePlayer.InputEnabled = false;
        while (DialogueSystem.Active)
        {
            yield return null;
        }

        // Fade to black
        yield return StartCoroutine(FindObjectOfType<LevelManager>().FadeCoroutine(false, LevelManager.Settings.baseFadeSpeed));

        DestroyBoulder();

        // Fade back
        yield return StartCoroutine(FindObjectOfType<LevelManager>().FadeCoroutine(true, LevelManager.Settings.baseFadeSpeed/2f));

        LevelManager.activePlayer.InputEnabled = true;
    }

    private void DestroyBoulder()
    {
        C_Grid grid = FindObjectOfType<C_Grid>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                grid.NodeFromWorldPoint(transform.position + new Vector3(i * grid.nodeRadius * 2, j * grid.nodeRadius * 2)).currentObject = null;
            }
        }
        GetComponentInChildren<SpriteRenderer>().sprite = destroyedSprite;
    }
}
