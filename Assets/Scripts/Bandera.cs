using UnityEngine;

public class Bandera : MonoBehaviour
{
    public GestorVictoria gestorVictoria;
    
    void Start()
    {
        Collider2D collider2D = GetComponent<Collider2D>();
        
        if (collider2D != null)
        {
            collider2D.isTrigger = true;
        }
        else
        {
            Debug.LogError("No hay colider");
        }
        
        if (gestorVictoria == null)
            gestorVictoria = FindObjectOfType<GestorVictoria>();
    }
    
    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Player") && gestorVictoria != null)
        {
            gestorVictoria.MostrarPantallaVictoria();
        }
    }
}