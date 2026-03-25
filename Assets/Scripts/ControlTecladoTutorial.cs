using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlTecladoTutorial : MonoBehaviour
{
    public GestorVictoria gestorVictoria;
    
    void Start()
    {
        if (gestorVictoria == null)
        {
            gestorVictoria = FindObjectOfType<GestorVictoria>();
        }
    }
    
    void Update()
    {
        bool victoriaActiva = false;
        if (gestorVictoria != null && gestorVictoria.pantallaVictoria != null)
        {
            victoriaActiva = gestorVictoria.pantallaVictoria.activeSelf;
        }
        if (victoriaActiva)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarNivel();
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IrAlMenu();
            }
        }
    }
    
    void ReiniciarNivel()
    {
        ResetearTiempo();
        
        if (gestorVictoria != null)
        {
            gestorVictoria.ResetearTiempo();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void IrAlMenu()
    {
        ResetearTiempo();
        SceneManager.LoadScene(0);
    }
    
    void ResetearTiempo()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}