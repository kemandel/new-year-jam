using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    public Dialogue[] dialogue;

    private void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dialogueSystem.PlayDialogue(dialogue);
        }
        gameObject.SetActive(false);
    }

}
