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
    private int characterLineLimit = 32; //18

    private AudioSource voiceSource;
    //UI for dialogue
    private static Canvas dialogueUI;

    private SoundManager soundManager;
    //dialogue active status
    public static bool Active { get; private set; }

    bool pause = false;

    private AudioClip[] voiceNotes;

    private void Start()
    {
        //set the audio source we want to play from
        soundManager = FindObjectOfType<SoundManager>();
        voiceSource = soundManager.dialogueSource;
        voiceNotes = Resources.LoadAll<AudioClip>("DialogueNotes");
    }
    private void Awake()
    {
        dialogueUI = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Canvas>();
        dialogueUI.enabled = false;
    }

    private void Update()
    {
        if (Active)
        {
            if (pause && Input.anyKeyDown){
                pause = false;
            }
        }
    }
    public void PlayDialogue(Dialogue[] dialogue)
    {
        dialogueUI.enabled = true;
        StartCoroutine(DialogueCoroutine(dialogue));
    }

    private IEnumerator DialogueCoroutine(Dialogue[] dialogue)
    {
        //fade background music out
         StartCoroutine(soundManager.fadeAudio(2f, 0.3f));

        Active = true;
        Dialogue currentDialogue;

        //Get the dialogueUI's text component
        TMP_Text currentText = dialogueUI.GetComponentInChildren<TMP_Text>();
        for (int i = 0; i < dialogue.Length; i++)
        {
            //get previous speaker if there is one
            string previousSpeaker = i > 0 ? dialogue[i - 1].speaker : "";

            currentDialogue = dialogue[i];
            voiceSource.clip = currentDialogue.voice;

            //if new speaker, add name at the top
            if (previousSpeaker != currentDialogue.speaker)
            {
                //if playing as Sam, should be in 2nd person so no name printed
                currentText.text = currentDialogue.speaker == "" ? "" : currentDialogue.speaker + ':' + '\n';
            }
            else
            {
                currentText.text = "";
            }
            //dialogue before adding current dialogue
            //string startingDialogue = currentText.text;

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
                    currentText.text = activeDialogue + dialogueWords[j].Substring(0, k + 1);
                    float delay = .05f;

                    //play sound of character for each dialogue line
                    int randNum = Random.Range(0, voiceNotes.Length);
                    voiceSource.clip = voiceNotes[randNum];
                    voiceSource.Play(); //need to figure out if this is looping or what, if so stop it

                    yield return new WaitForSeconds(delay);
                }

                if (j < dialogueWords.Length - 1)
                {
                    currentText.text += " ";
                }
            }

            pause = true;

            while (pause)
            {
                yield return null;
            }
        }

        dialogueUI.enabled = false;
        voiceSource.Stop();
        Active = false;

        //fade background music back in
        yield return StartCoroutine(soundManager.fadeAudio(2f));
    }

}
[System.Serializable]
public struct Dialogue
{
    public string speaker;
    public string dialogueText;
    public AudioClip voice;
}
