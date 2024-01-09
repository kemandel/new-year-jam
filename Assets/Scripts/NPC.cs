using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public PlayerController playerToControl;

    public Dialogue[] beforeDialogue;
    public Dialogue[] afterDialogue;
    public TextAsset minigameData;

    void Start()
    {
        //FindObjectOfType<MinigameController>().StartGame(minigameData);
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
    }

    public void Interact()
    {
        StartCoroutine(NPCCoroutine());
    }

    private IEnumerator NPCCoroutine()
    {
        LevelManager.activePlayer.InputEnabled = false;
        FindObjectOfType<DialogueSystem>().PlayDialogue(beforeDialogue);
        while(DialogueSystem.Active)
        {
            yield return null;
        }
        FindObjectOfType<MinigameController>().StartGame(minigameData);
        while (MinigameController.Active)
        {
            yield return null;
        }
        FindObjectOfType<DialogueSystem>().PlayDialogue(afterDialogue);
        while(DialogueSystem.Active)
        {
            yield return null;
        }
        LevelManager.AddPlayer(Instantiate(playerToControl, transform.position, Quaternion.identity));
        LevelManager.activePlayer.InputEnabled = true;
        Destroy(gameObject);
    }
}
