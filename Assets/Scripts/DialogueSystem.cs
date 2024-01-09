using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueSystem : MonoBehaviour
{

    /// <summary>
    /// The amount of characters that can fit on each line of the dialogue box
    /// </summary>
    public int characterLineLimit = 18;
    public AudioSource voiceSource;
    //UI for dialogue
    private static Canvas dialogueUI;

    private void Awake()
    {
        dialogueUI = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Canvas>();
        dialogueUI.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDialogue(Dialogue[] dialogue)
    {
        dialogueUI.enabled = true;
        StartCoroutine(DialogueCoroutine(dialogue));
    }

    private IEnumerator DialogueCoroutine(Dialogue[] dialogue)
    {
        Dialogue currentDialogue;
        //Get the dialogueUI's text component
        TMP_Text currentText = dialogueUI.GetComponentInChildren<TMP_Text>();
        bool pause = false;
        for (int i = 0; i < dialogue.Length; i++)
        {
            currentDialogue = dialogue[i];
            currentText.text = currentDialogue.speaker + '\n';

            //dialogue before adding current dialogue
            string startingDialogue = currentText.text;

            string[] dialogueWords = currentDialogue.dialogueText.Split(' ');

            //for each word in dialogue
            for (int j = 0; j < dialogueWords.Length; j++)
            {
                //add a new line if word is too long
                string[] lines = currentText.text.Split('\n');
                int charsInLine = lines[lines.Length - 1].Length;
                if (charsInLine + dialogueWords[j].Length > characterLineLimit)
                {
                    currentText.text += '\n';
                }

                //dialogue after each word
                string activeDialogue = currentText.text;
                //for each char in word
                for (int k = 0; k < dialogueWords[j].Length; k++)
                {
                    currentText.text = activeDialogue + dialogueWords[j].Substring(k + 1);
                    voiceSource.clip = currentDialogue.voice;
                    voiceSource.Play(); //need to figure out if this is looping or what, if so stop it
                }

                if (j < dialogueWords.Length - 1)
                {
                    currentText.text += " ";
                }

                pause = true;

                while (pause)
                {
                    if (Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        pause = false;
                    }
                    yield return null;
                }
            }
        }
        dialogueUI.enabled = false;
        voiceSource.Stop();
    }

}
[System.Serializable]
public struct Dialogue
{
    public string speaker;
    public string dialogueText;
    public AudioClip voice;
}
