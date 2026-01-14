using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public GameObject contButton; 
    public float wordSpeed;
    public bool playerIsClose;

    void Update()
    {
        // Tu lógica original de abrir con E
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        // NUEVO: Avanzar con ESPACIO (solo si el panel está activo)
        // He mantenido tu IF original de comparación de texto
        if (dialoguePanel.activeInHierarchy && dialogueText.text == dialogue[index]) 
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
        // CORRECCIÓN LÍNEA 41: 'text' en minúscula para que no dé error CS1061
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

    // Tu corrutina original tal cual la tenías
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