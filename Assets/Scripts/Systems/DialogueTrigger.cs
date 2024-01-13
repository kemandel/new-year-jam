using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    public Dialogue[] dialogue;
    public string[] necessaryCharacters;
    private bool conditionsMet;

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
        if (collision.gameObject.CompareTag("Player") && conditionsMet)
        {
            dialogueSystem.PlayDialogue(dialogue);
            gameObject.SetActive(false);
        }
    }

}
