using UnityEngine;

public class Bandera : MonoBehaviour
{
    [SerializeField] private GestorVictoria gestorVictoria;

    private void Awake()
    {
        if (gestorVictoria == null)
        {
            gestorVictoria = FindObjectOfType<GestorVictoria>();
        }
    }

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
        {
            col.isTrigger = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;

        if (gestorVictoria != null)
        {
            gestorVictoria.MostrarPantallaVictoria();
        }
    }
}