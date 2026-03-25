using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    /*
    BEFORE: 
    public Text nameText;
    public Text dialogueText;
     */
    // AFTER:
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    [SerializeField] GameObject dialogueBox;

    public static bool dialogueIsActive = false;

    private void Awake()
    {
        Debug.Assert(dialogueBox != null, "Insert a dialog box!");
    }

    public Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }
    void Update()
    {
        if (dialogueIsActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence();
        }
    }
    public void StartDialogue (Dialogue dialogue)
    {
        dialogueIsActive = true;
        dialogueBox.SetActive(true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    void EndDialogue()
    {
        dialogueIsActive = false;
        dialogueBox.SetActive(false);
    }

}
