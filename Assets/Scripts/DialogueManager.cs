using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI��� - inspector���� ����")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("�⺻����")]
    public Sprite defaultCharacterImage;

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnclick = true;


    private NewDialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialougueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;



    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)
            {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);

            }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping=false;

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }

        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }


    void EndDialogue()
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine );
            typingCoroutine = null;
        }

        isDialougueActive = false;
        isTyping = false;
        DialoguePanel.SetActive( false );
        currentLineIndex = 0;
    }



    public void ShowNextLine()
    {
        currentLineIndex++;

        if(currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    public void HandleNextInput()
    {
        if(isTyping && skipTypingOnclick)
        {
            CompleteTyping();
        }
        else if(!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDialougue()
    {
        EndDialogue() ;
    }


    public bool IsDialougueActive()
    {
        return isDialougueActive;
    }
    public void StartDialogue(NewDialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialougueActive= true;

        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if (dialogue.characterImage != null)
        {
            characterImage.sprite = dialogue.characterImage;
        }
        else
        {
            characterImage.sprite = defaultCharacterImage;
        }

        ShowCurrentLine();
    }



    // Start is called before the first frame update
    void Start()
    {

        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialougueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }
}
