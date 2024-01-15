using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : NodeObject, IInteractable
{
    public Dialogue[] noTobyDialogue;
    public Dialogue[] useTobyDialogue;
    public Dialogue[] openDoorDialogue;

    // Start is called before the first frame update
    public override void Start()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position + new Vector3(0,FindObjectOfType<C_Grid>().nodeRadius * 2)).currentObject = gameObject;
    }

    public void Interact()
    {
        if (!LevelManager.CheckForCharacter("Toby"))
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(noTobyDialogue);
            return;
        }

        if (LevelManager.activePlayer.characterName != "Toby")
        {
            FindObjectOfType<DialogueSystem>().PlayDialogue(useTobyDialogue);
            return;
        }

        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        FindObjectOfType<DialogueSystem>().PlayDialogue(openDoorDialogue);
        LevelManager.activePlayer.InputEnabled = false;
        while (DialogueSystem.Active)
        {
            yield return null;
        }

        // Fade to black
        yield return StartCoroutine(FindObjectOfType<LevelManager>().FadeCoroutine(false, LevelManager.Settings.baseFadeSpeed));

        OpenDoor();

        // Fade back
        yield return StartCoroutine(FindObjectOfType<LevelManager>().FadeCoroutine(true, LevelManager.Settings.baseFadeSpeed/2f));

        LevelManager.activePlayer.InputEnabled = true;
    }

    private void OpenDoor()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = null;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position + new Vector3(0,FindObjectOfType<C_Grid>().nodeRadius * 2)).currentObject = null;
        GetComponentInChildren<SpriteRenderer>().sprite = null;
    }
}
