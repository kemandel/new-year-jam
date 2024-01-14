using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    public Dialogue[] dialogue;
    public string[] necessaryCharacters;
    private bool conditionsMet;
    private bool canActivate = true;

    public bool endGame = false;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        conditionsMet = true;
        foreach (string charName in necessaryCharacters)
        {
            if (!LevelManager.CheckForCharacter(charName))
            {
                //char not added yet
                conditionsMet = false;
            }
        }
        if (collision.gameObject.CompareTag("Player") && conditionsMet && canActivate)
        {
            StartCoroutine(DialogueCoroutine());
        }
    }

    private IEnumerator DialogueCoroutine()
    {
        canActivate = false;
        dialogueSystem.PlayDialogue(dialogue);
        while (DialogueSystem.Active)
        {
            yield return null;
        }
        if (endGame)
        {
            GameObject.Find("GameCompleteCanvas").GetComponent<Canvas>().enabled = true;
        }
        gameObject.SetActive(false);
    }

}
