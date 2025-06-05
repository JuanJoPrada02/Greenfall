using System.Collections;
using System.Data;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

using UnityEngine;
using System;

using UnityEngine.UIElements;


public class NPC : MonoBehaviour
{
   
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;
    [SerializeField] private TMP_Text dialogueText;


    
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject carta;
    
    
    



    private bool IsPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private float typingTime = 0.05f;

    

    private void Update()
    {
        if (IsPlayerInRange && Input.GetButtonDown("Fire1"))
        {
            if (!didDialogueStart)
            {
                StartDialogue();

            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
                
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
                

            }
        }
    }
    private void StartDialogue()
    {
        didDialogueStart = true;    
        dialoguePanel.SetActive(true);
        carta.SetActive(false );
        Time.timeScale = 0f;
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }
    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex < dialogueLines.Length)
        {     
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

   
    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsPlayerInRange = true;
            carta.SetActive(true);
            
            Debug.Log("hola");
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            IsPlayerInRange = false;
            carta.SetActive(false);
            
            Debug.Log("chao");
        }
        
    }
}
