using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_NPC : MonoBehaviour
{
    public NewDialogue myDialogue;
    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager == null )
        {
            Debug.LogError("���̾� �α� �Ŵ����� �����ϴ�.");
        }
    }

    void OnMouseDown()
    {
        if (dialogueManager == null) return;
        if (dialogueManager.IsDialougueActive()) return;
        if (myDialogue == null) return;


        dialogueManager.StartDialogue(myDialogue);
    }

}
