using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewDialogue", menuName = "Dialogue/DialogueData")]
public class NewDialogue : ScriptableObject
{
    [Header("캐릭터 정보")]
    public string characterName = "캐릭터";
    public Sprite characterImage;

    [Header("대화 내용")]
    [TextArea(3,10)]
    public List<string> dialogueLines = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
