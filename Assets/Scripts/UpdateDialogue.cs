using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDialogue : MonoBehaviour
{
    public Dialogue[] mabelDialogue;
    bool updatedDialogue;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.AddPlayerEvent += UpdateCharacterDialogue;
    }

    void UpdateCharacterDialogue(string characterName)
    {
        if (LevelManager.CheckForCharacter("Mabel") && !updatedDialogue)
        {
            Debug.Log("Mabel has been found");
            if (gameObject.GetComponent<NPC>() != null)
                gameObject.GetComponent<NPC>().beforeDialogue = mabelDialogue;
            updatedDialogue = true;
        }
    }
}
