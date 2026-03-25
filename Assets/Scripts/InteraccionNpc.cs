using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteraccionNpc : MonoBehaviour
{
    [SerializeField]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    public float wordSpeed = 0.03f;
    private int index;
    public GameObject textPressE; 
    public GameObject contButton;
    public bool playerIsClose;
    public Movimientojugador Scriptjugador;
    public int nHabilidad;

    void Update()
    {
        if (playerIsClose)
        {
            textPressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    zeroText();
                }
                else
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                    Scriptjugador.Habilidad = nHabilidad;
                    
                }
            }
        } else {
            textPressE.SetActive(false);
        }
        
        if (dialoguePanel.activeInHierarchy &&
            dialogue.Length > 0 &&
            index >= 0 &&
            index < dialogue.Length &&
            dialogueText.text == dialogue[index]) 
        {
            if (contButton != null) contButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLine();
            }
        }
    }

    public void zeroText() 
    {
        StopAllCoroutines();
        dialogueText.text = ""; 
        index = 0;
        dialoguePanel.SetActive(false);
    }

    public void NextLine()
    {
        if (contButton != null) contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

}